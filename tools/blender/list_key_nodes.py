import bpy, sys
glb = sys.argv[sys.argv.index("--") + 1]
bpy.ops.object.select_all(action="SELECT")
bpy.ops.object.delete(use_global=False)
bpy.ops.import_scene.gltf(filepath=glb)
keys = ("deposit", "partition", "manager", "bathroom", "sign", "ext_", "roof", "porch", "stair", "floor_")
for obj in sorted(bpy.data.objects, key=lambda o: o.name):
    n = obj.name.lower()
    if any(k in n for k in keys):
        v = len(obj.data.vertices) if obj.type == "MESH" and obj.data else 0
        print(f"{obj.name} verts={v}")
