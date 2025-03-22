class_name BackgroundLoading

static var _requested = {}
static var _threads = []

static func request(path, type_hint = "", use_sub_threads = false, cache_mode = ResourceLoader.CACHE_MODE_REUSE):
	if _requested.has(path):
		return ERR_ALREADY_EXISTS

	var error = ResourceLoader.load_threaded_request(path, type_hint, use_sub_threads, cache_mode)
	if error != OK:
		return error

	_requested[path] = null
	return OK

static func requested():
	return _requested.keys()

static func get_resource(path):
	if !ResourceLoader.exists(path):
		return Resource.new()

	_requested.erase(path);
	return ResourceLoader.load_threaded_get(path)

static func get_status(path):
	if _requested.has(path):
		return ResourceLoader.load_threaded_get_status(path)

	return ResourceLoader.THREAD_LOAD_INVALID_RESOURCE

static func cancel(path):
	_requested.erase(path)

	if ResourceLoader.load_threaded_get_status(path) == ResourceLoader.THREAD_LOAD_LOADED:
		ResourceLoader.load_threaded_get(path)
	elif ResourceLoader.load_threaded_get_status(path) == ResourceLoader.THREAD_LOAD_IN_PROGRESS:
		clear_finished_threads()
		_threads.push_back(Thread.new())
		_threads[-1].start(func(): ResourceLoader.load_threaded_get(path))

static func clear():
	for path in _requested.keys():
		cancel(path)

	_requested.clear()

static func clear_finished_threads():
	for thread in _threads:
		if !thread.is_alive():
			_threads.erase(thread)
