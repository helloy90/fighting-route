using Godot;
using System;

public interface Interactable
{
	string GetTypeObj();
	string GetLabel();
	string GetDescription();
	string GetPrompt();
	void Interact();
}
