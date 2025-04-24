using Godot;

[GlobalClass]
public partial class LevelResource : Resource {

	[Export] public string _Name = "Level";
	[Export] public PackedScene _Scene;

}
