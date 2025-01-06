using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Node
{

	private class inventory_object{
		private string label;
		private string description;
		private int count;

		public inventory_object(string label, string description){
			this.count = 1;
			this.description = description;
			this.label = label;
		}

		public void addNumber(){
			count += 1;
		}
	}

	private Dictionary<int, inventory_object> bag;
	private Label info_bag;

	public void add(int key, string label, string description){
		if(bag.ContainsKey(key)){
			bag[key].addNumber();
		} else {
			var new_inventory_obj = new inventory_object(label, description);
			bag[key] = new_inventory_obj;
		}
	}

    public override void _Ready()
    {
        info_bag = GetNode("InfoBag") as Label;
		bag =  new Dictionary<int, inventory_object>();
    }

    public override void _PhysicsProcess(double delta)
    {
        info_bag.Text = "bag \n" + "weapon: ";
    }
}

//has
//remove
//getKey
//get all
