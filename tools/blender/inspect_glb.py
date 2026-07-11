"""Inspeciona GLB importado - verifica bevel/vertex counts."""
import bpy
import sys

glb = sys.argv[sys.argv.index("--") + 1]
bpy.ops.object.select_all(action="SELECT")
bpy.ops.object.delete(use_global=False)

bpy.ops.import_scene.gltf(filepath=glb)
print(f"=== GLB INSPECTION: {glb} ===")
print(f"Objects: {len(bpy.data.objects)}")

bevel_like = []
low_poly_boxes = []
for obj in bpy.data.objects:
    if obj.type != "MESH":
        continue
    v = len(obj.data.vertices)
    name = obj.name.lower()
    if v <= 12 and ("wall" in name or "roof" in name or "ext_" in name or "floor" in name):
        low_poly_boxes.append(f"{obj.name}:{v}")
    if v > 50 and v < 500:
        bevel_like.append(f"{obj.name}:{v}")

print(f"Low-poly boxes (<=12 verts): {len(low_poly_boxes)}")
for x in low_poly_boxes[:15]:
    print(f"  {x}")
print(f"Bevel-like meshes (50-500 verts): {len(bevel_like)}")
for x in bevel_like[:15]:
    print(f"  {x}")

collections = set()
for obj in bpy.data.objects:
    for c in obj.users_collection:
        collections.add(c.name)
print(f"Collections in GLB: {sorted(collections)}")
