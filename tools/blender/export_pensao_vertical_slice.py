"""Consolida a vertical slice da Pensao a partir do GLB valido.

Uso:
  blender --background --python tools/blender/export_pensao_vertical_slice.py -- fonte.glb oficial.blend oficial.glb

O script reorganiza objetos em collections oficiais, remove guides/cameras/lights,
salva um .blend editavel e exporta um GLB somente visual, sem colisoes automaticas.
"""

from pathlib import Path
import sys
import bpy


COLLECTION_RULES = (
    ("World_Ground", ("ground_", "entry_ground")),
    ("World_Path", ("central_path", "path_plank")),
    ("World_Cliffs", ("cliff_",)),
    ("World_Fence", ("fence_",)),
    ("World_Pension_Exterior", ("ext_", "roof_", "porch_", "front_", "sign_pensao")),
    ("World_Pension_Interior", (
        "floor_", "reception_", "guest_", "job_contract", "corridor_", "room102_",
        "left_backroom", "kitchen_", "deposit_", "stair_", "manager_", "bathroom_", "locked_",
    )),
    ("World_Vegetation", ("cactus_", "dry_bush")),
    ("World_Background", ("rear_ground", "bg_", "moon_", "star_")),
    ("World_Props", ("job_offer", "rust_", "bull_", "shadow_")),
)


def arguments():
    args = sys.argv[sys.argv.index("--") + 1:] if "--" in sys.argv else []
    if len(args) != 3:
        raise SystemExit("Informe fonte.glb, destino.blend e destino.glb.")
    return tuple(Path(value).resolve() for value in args)


def clear_scene():
    bpy.ops.object.select_all(action="SELECT")
    bpy.ops.object.delete(use_global=False)
    for collection in list(bpy.data.collections):
        bpy.data.collections.remove(collection)


def remove_non_visual():
    removed = []
    for obj in list(bpy.data.objects):
        normalized = obj.name.lower()
        if obj.type in {"CAMERA", "LIGHT"} or normalized.startswith("guide_"):
            removed.append(obj.name)
            bpy.data.objects.remove(obj, do_unlink=True)
    return removed


def official_collection_for(obj):
    normalized = obj.name.lower()
    for collection_name, prefixes in COLLECTION_RULES:
        if normalized.startswith(prefixes):
            return collection_name
    return "World_Props"


def organize_collections():
    collections = {}
    for collection_name, _ in COLLECTION_RULES:
        collection = bpy.data.collections.new(collection_name)
        bpy.context.scene.collection.children.link(collection)
        collections[collection_name] = collection

    for obj in list(bpy.context.scene.objects):
        target = collections[official_collection_for(obj)]
        for current in list(obj.users_collection):
            current.objects.unlink(obj)
        target.objects.link(obj)


def save_blend(destination):
    destination.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.wm.save_as_mainfile(filepath=str(destination), check_existing=False)


def export_glb(destination):
    destination.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.object.select_all(action="DESELECT")
    for obj in bpy.context.scene.objects:
        if obj.type in {"MESH", "EMPTY"}:
            obj.select_set(True)
    bpy.ops.export_scene.gltf(
        filepath=str(destination),
        export_format="GLB",
        use_selection=True,
        export_apply=True,
        export_cameras=False,
        export_lights=False,
        export_extras=False,
        export_materials="EXPORT",
    )


if __name__ == "__main__":
    source, blend_destination, glb_destination = arguments()
    if not source.exists():
        raise FileNotFoundError(source)
    clear_scene()
    bpy.ops.import_scene.gltf(filepath=str(source))
    removed = remove_non_visual()
    organize_collections()
    save_blend(blend_destination)
    export_glb(glb_destination)
    print(f"BLEND_OFICIAL={blend_destination}")
    print(f"GLB_OFICIAL={glb_destination}")
    print(f"REMOVIDOS={len(removed)}:{','.join(removed)}")
