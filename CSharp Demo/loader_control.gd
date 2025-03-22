extends AspectRatioContainer

@export var cached_label: Label
@export var requested_label: Label
@export var instantiated_label: Label

func update(cached: String, requested: String, instantiated: String):
	cached_label.text = cached
	requested_label.text = requested
	instantiated_label.text = instantiated
