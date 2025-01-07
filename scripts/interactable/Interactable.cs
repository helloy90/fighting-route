using Godot;
using System;

public interface Interactable
{
	string GetLabel();
    string GetDescription();
    string GetInterfaceText();

    void Interact();
    void PlayAnimation(string animationName);
}
