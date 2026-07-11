"""Exporta o GLB oficial da vertical slice a partir do .blend fonte.

Uso:
  blender --background assets/blender/trail_intro_pensao_vertical_slice_v01.blend \
    --python tools/blender/export_vertical_slice_from_blend.py -- \
    assets/models/levels/pensao_santa_luzia/pensao_santa_luzia_vertical_slice_v01.glb

Exporta somente collections visuais uteis. Remove cameras, luzes e guides.
"""

from pathlib import Path
import sys
import bpy

VISUAL_COLLECTIONS = (
    "World_Ground",
    "World_Path",
    "World_Cliffs",
    "World_Fence",
    "World_Pension_Exterior",
    "World_Pension_Interior",
    "World_Props",
    "World_Vegetation",
    "World_Background",
)

SKIP_COLLECTION_PREFIXES = (
    "World_Lights",
    "World_Cameras",
    "Godot_Guides",
)


def arguments():
    args = sys.argv[sys.argv.index("--") + 1:] if "--" in sys.argv else []
    if len(args) != 1:
        raise SystemExit("Informe destino.glb.")
    return Path(args[0]).resolve()


def should_skip_collection(name: str) -> bool:
    return any(name.startswith(prefix) for prefix in SKIP_COLLECTION_PREFIXES)


def should_skip_object(obj) -> bool:
    normalized = obj.name.lower()
    if obj.type in {"CAMERA", "LIGHT"}:
        return True
    if normalized.startswith("guide_"):
        return True
    for collection in obj.users_collection:
        if should_skip_collection(collection.name):
            return True
    return False


def remove_non_visual():
    removed = []
    for obj in list(bpy.data.objects):
        if should_skip_object(obj):
            removed.append(obj.name)
            bpy.data.objects.remove(obj, do_unlink=True)
    return removed


def collect_export_objects():
    export_objects = []
    for collection_name in VISUAL_COLLECTIONS:
        collection = bpy.data.collections.get(collection_name)
        if collection is None:
            continue
        for obj in collection.all_objects:
            if obj.type in {"MESH", "EMPTY"} and obj.name not in {entry.name for entry in export_objects}:
                export_objects.append(obj)
    if export_objects:
        return export_objects

    # Fallback: exporta meshes fora de collections proibidas.
    for obj in bpy.data.objects:
        if obj.type in {"MESH", "EMPTY"} and not should_skip_object(obj):
            export_objects.append(obj)
    return export_objects


def export_glb(destination: Path, export_objects):
    destination.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.object.select_all(action="DESELECT")
    for obj in export_objects:
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
    glb_destination = arguments()
    removed = remove_non_visual()
    export_objects = collect_export_objects()
    if not export_objects:
        raise RuntimeError("Nenhum objeto visual encontrado para exportar.")
    export_glb(glb_destination, export_objects)
    print(f"GLB_OFICIAL={glb_destination}")
    print(f"OBJETOS_EXPORTADOS={len(export_objects)}")
    print(f"REMOVIDOS={len(removed)}:{','.join(removed)}")
