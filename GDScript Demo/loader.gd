extends Node

signal update_text(cached, requested, instantiated)

var _instantiated = []

func request_scene(path):
	BackgroundLoading.request(path)

func cancel_request(path):
	BackgroundLoading.cancel(path)

func instantiate_scene(path):
	var scene = BackgroundLoading.get_resource(path)
	if scene != null:
		add_child(scene.instantiate())
		_instantiated.push_back(path)

func clear():
	for child in get_children():
		child.queue_free()

	BackgroundLoading.clear()
	_instantiated.clear()

var _last_update_time = 0
func _process(_delta):
	var current_time = Time.get_ticks_msec()
	if _last_update_time + 100 > current_time:
		return

	_last_update_time = current_time

	const path1 = "res://scene_1.tscn"
	const path2 = "res://scene_2.tscn"

	var instantiated = " ".join(_instantiated)
	var requested = " ".join(BackgroundLoading.requested())
	var cached = (path1 if ResourceLoader.has_cached(path1) else "") + " " + (path2 if ResourceLoader.has_cached(path2) else "")
	if instantiated == "": instantiated = "None"
	if requested == "": requested = "None"
	if cached == " ": cached = "None"

	update_text.emit(cached, requested, instantiated);

func _input(event):
	if event.is_action_pressed("ui_left"):
		print(BackgroundLoading.requested())
