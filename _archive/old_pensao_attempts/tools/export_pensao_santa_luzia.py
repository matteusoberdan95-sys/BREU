"""Exporta a Pensao Santa Luzia integrada sem elementos exclusivos do Blender.

Uso:
  blender --background --python tools/blender/export_pensao_santa_luzia.py -- <entrada.blend|entrada.glb> <saida.glb>

Este utilitario fica preservado para uma sprint futura. A estabilizacao atual nao
reexporta nem altera geometria: a cena Godot usa o GLB visual anterior.
"""

from pathlib import Path
import sys
import bpy


EXCLUDED_COLLECTIONS = {
    "World_Fog",
    "World_Cameras",
    "World_Lights",
    "World_Guides",
}
EXCLUDED_NAME_PREFIXES = ("fog_", "guide_")
EXCLUDED_OBJECT_NAMES = {"camera", "light"}


def arguments():
    args = sys.argv[sys.argv.index("--") + 1 :] if "--" in sys.argv else []
    if len(args) != 2:
        raise SystemExit("Informe entrada (.blend ou .glb) e saida (.glb).")
    return Path(args[0]).resolve(), Path(args[1]).resolve()


def load_source(source: Path):
    if not source.exists():
        raise FileNotFoundError(source)
    if source.suffix.lower() == ".blend":
        bpy.ops.wm.open_mainfile(filepath=str(source))
    elif source.suffix.lower() in {".glb", ".gltf"}:
        bpy.ops.object.select_all(action="SELECT")
        bpy.ops.object.delete(use_global=False)
        bpy.ops.import_scene.gltf(filepath=str(source))
    else:
        raise ValueError("A entrada deve ser .blend, .glb ou .gltf.")


def collection_is_excluded(collection):
    current = collection
    while current:
        if current.name in EXCLUDED_COLLECTIONS:
            return True
        current = next((c for c in bpy.data.collections if current.name in c.children), None)
    return False


def remove_excluded_objects():
    removed = []
    for obj in list(bpy.data.objects):
        in_excluded_collection = any(collection_is_excluded(c) for c in obj.users_collection)
        normalized_name = obj.name.lower()
        excluded_by_name = (
            normalized_name.startswith(EXCLUDED_NAME_PREFIXES)
            or normalized_name in EXCLUDED_OBJECT_NAMES
        )
        excluded_by_type = obj.type in {"CAMERA", "LIGHT"}
        if in_excluded_collection or excluded_by_name or excluded_by_type:
            removed.append(obj.name)
            bpy.data.objects.remove(obj, do_unlink=True)
    return removed


def export_glb(destination: Path):
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
    source_path, destination_path = arguments()
    load_source(source_path)
    removed_objects = remove_excluded_objects()
    export_glb(destination_path)
    print(f"Exportado: {destination_path}")
    print(f"Objetos excluidos ({len(removed_objects)}): {', '.join(removed_objects)}")
