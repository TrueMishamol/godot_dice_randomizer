using Godot;
using Godot.Collections;

[GlobalClass]
public partial class DicePackResource : Resource {

	[Export] public string _Name = "Dice Pack";
	[Export] public Array<DiceResource> _DiceResources = new Array<DiceResource>();

}
