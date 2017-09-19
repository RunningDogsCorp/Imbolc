/*
 * GUIMenu.js
 * Copyright (C) 2011-2012 Zeemote. All rights reserved.
 *   This is a sample script.
 */
var _zgui : ZeemoteGUI;
private static var _showMenuFlag : boolean = true;

function OnGUI ()
{
	if(!_showMenuFlag)
		return;
		
	var orgFixedHeight : int  = GUI.skin.button.fixedHeight;
	GUI.skin.button.fixedHeight = 40;
	
	GUILayout.BeginArea (Rect (20, 20, 300, 300));
	
	/* Show Controller menu button. */
	if(!ZeemoteInput.IsConnected(1)) {
		if(GUILayout.Button ("Connect to Zeemote Controller"))
		{
			_zgui.ShowControllerMenu(1, "Controller");
		}
	}
	else {
		if(GUILayout.Button ("Disconnect from Zeemote Controller"))
		{
			_zgui.ShowControllerMenu(1, "Controller");
		}
	}
	/* Exit button. */
	if(GUILayout.Button ("Exit SampleApp"))
	{
		Debug.Log( "Exit" );
		ZeemoteInput.CleanupPlugin();
		Application.Quit();
	}

	GUILayout.EndArea();
	GUI.skin.button.fixedHeight = orgFixedHeight;
	
}
