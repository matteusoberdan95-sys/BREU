"""Exporta GLB oficial da vertical slice com modifiers aplicados.

Uso:
  blender --background assets/blender/trail_intro_pensao_vertical_slice_v01.blend \\
    --python tools/blender/export_vertical_slice_official.py

Se o .blend oficial estiver vazio (apenas cubo padrao), reconstrói a partir de
quarto-07/trail_intro_pensao_integrada_v01.blend filtrando collections validas.
"""

from __future__ import annotations

from pathlib import Path
import sys
import bpy

PROJECT_ROOT = Path(__file__).resolve().parents[2]
OFFICIAL_BLEND = PROJECT_ROOT / "assets/blender/trail_intro_pensao_vertical_slice_v01.blend"
FALLBACK_BLEND = Path(r"C:/Users/mober/OneDrive/Desktop/quarto-07/trail_intro_pensao_integrada_v01.blend")
GLB_OUTPUT = PROJECT_ROOT / "assets/models/levels/pensao_santa_luzia/pensao_santa_luzia_vertical_slice_v01.glb"

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
    "World_Guides",
    "World_Fog",
    "World_FogCards",
    "World_M01",
)

TEXT_PREFIXES = (
    "sign_",
    "label_",
    "text_",
    "job_offer_text",
    "reception_sign",
    "deposit_sign",
    "room102_sign",
)


def is_default_scene() -> bool:
    mesh_names = [obj.name for obj in bpy.data.objects if obj.type == "MESH"]
    return len(mesh_names) <= 1 and any(name.lower().startswith("cube") for name in mesh_names)


def should_skip_collection(name: str) -> bool:
    return any(name.startswith(prefix) for prefix in SKIP_COLLECTION_PREFIXES)


def should_skip_object(obj) -> bool:
    normalized = obj.name.lower()
    if obj.type in {"CAMERA", "LIGHT", "FONT", "CURVE"}:
        return True
    if normalized.startswith("guide_"):
        return True
    for collection in obj.users_collection:
        if should_skip_collection(collection.name):
            return True
    return False


def clear_scene():
    bpy.ops.object.select_all(action="SELECT")
    bpy.ops.object.delete(use_global=False)
    for collection in list(bpy.data.collections):
        bpy.data.collections.remove(collection)


def append_collection(source_blend: Path, collection_name: str):
    directory = str(source_blend).replace("\\", "/") + "/Collection/"
    bpy.ops.wm.append(
        filepath=f"{directory}{collection_name}",
        directory=directory,
        filename=collection_name,
    )


def rebuild_from_fallback() -> int:
    if not FALLBACK_BLEND.exists():
        raise FileNotFoundError(f"Fallback ausente: {FALLBACK_BLEND}")
    clear_scene()
    appended = 0
    for collection_name in VISUAL_COLLECTIONS:
        try:
            append_collection(FALLBACK_BLEND, collection_name)
            appended += 1
        except RuntimeError:
            print(f"WARN collection ausente no fallback: {collection_name}")
    return appended


def remove_non_visual():
    removed = []
    for obj in list(bpy.data.objects):
        if should_skip_object(obj):
            removed.append(obj.name)
            bpy.data.objects.remove(obj, do_unlink=True)
    return removed


def hide_text_meshes():
    hidden = []
    for obj in bpy.data.objects:
        if obj.type != "MESH":
            continue
        normalized = obj.name.lower()
        if normalized.startswith(TEXT_PREFIXES) or "text" in normalized and "texture" not in normalized:
            obj.hide_render = True
            obj.hide_viewport = True
            hidden.append(obj.name)
    return hidden


def apply_object_for_export(obj):
    bpy.ops.object.select_all(action="DESELECT")
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj
    if obj.parent is None:
        try:
            bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
        except RuntimeError:
            pass
    for modifier in list(obj.modifiers):
        try:
            bpy.ops.object.modifier_apply(modifier=modifier.name)
        except RuntimeError as error:
            print(f"WARN modifier nao aplicado em {obj.name}: {error}")


def collect_export_objects():
    export_objects = []
    seen = set()
    for collection_name in VISUAL_COLLECTIONS:
        collection = bpy.data.collections.get(collection_name)
        if collection is None:
            continue
        for obj in collection.all_objects:
            if obj.type not in {"MESH", "EMPTY"}:
                continue
            if should_skip_object(obj):
                continue
            if obj.name in seen:
                continue
            seen.add(obj.name)
            export_objects.append(obj)
    if export_objects:
        return export_objects
    return [
        obj
        for obj in bpy.data.objects
        if obj.type in {"MESH", "EMPTY"} and not should_skip_object(obj)
    ]


def save_official_blend():
    OFFICIAL_BLEND.parent.mkdir(parents=True, exist_ok=True)
    bpy.ops.wm.save_as_mainfile(filepath=str(OFFICIAL_BLEND), check_existing=False)


def export_glb(export_objects):
    GLB_OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    for obj in export_objects:
        if obj.type == "MESH":
            apply_object_for_export(obj)
    bpy.ops.object.select_all(action="DESELECT")
    for obj in export_objects:
        if obj.name in bpy.data.objects:
            obj.select_set(True)
    bpy.ops.export_scene.gltf(
        filepath=str(GLB_OUTPUT),
        export_format="GLB",
        use_selection=True,
        export_apply=True,
        export_cameras=False,
        export_lights=False,
        export_extras=False,
        export_materials="EXPORT",
        export_texcoords=True,
        export_normals=True,
        export_tangents=False,
    )


def main():
    rebuilt = False
    if is_default_scene():
        print("WARN blend oficial vazio; reconstruindo do fallback integrada filtrada.")
        count = rebuild_from_fallback()
        if count == 0:
            raise RuntimeError("Nao foi possivel reconstruir o blend oficial.")
        rebuilt = True
        save_official_blend()

    removed = remove_non_visual()
    hidden_text = hide_text_meshes()
    export_objects = collect_export_objects()
    if not export_objects:
        raise RuntimeError("Nenhum objeto visual para exportar.")

    export_glb(export_objects)
    save_official_blend()

    print(f"REBUILT={rebuilt}")
    print(f"BLEND_OFICIAL={OFFICIAL_BLEND}")
    print(f"GLB_OFICIAL={GLB_OUTPUT}")
    print(f"OBJETOS_EXPORTADOS={len(export_objects)}")
    print(f"REMOVIDOS={len(removed)}")
    print(f"TEXTOS_OCULTOS={len(hidden_text)}")


if __name__ == "__main__":
    main()
