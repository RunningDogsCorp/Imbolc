/*
 * ZeemoteGUI.js
 * Copyright (C) 2011-2012 Zeemote. All rights reserved.
 *   This is a sample script.
 */

/*
 * Property
 */

/* Menu Item Size */
/* メニューアイテムのサイズ */
var MenuItemWidth: int = 350;
var MenuItemHeight : int = 30;

/* String Data */
/* 文字列データ */
var ButtonLabel_Cancel : String = "Cancel";	// Cancel Button
var ButtonLabel_Disconnect : String = "Disconnect";	// Disconenct Button

var DialogMessage_Connecting_prefix : String = "Connecting to ";	// Connecting Message Prefix
var DialogMessage_Connected_prefix : String =	"Connected to ";	// Connected Message Prefix
var DialogMessage_ConnectError : String = "Unable to connect. Make sure your controller is on.";	// Connect Error Message

var DialogMessage_Disconnecting : String = "Disconnecting.";	// Disconnecting Mesage
var DialogMessage_Disconnected : String = "Disconnected.";		// Disconnected Message
var DialogMessage_DisconnectError : String = "Unable to disconnect. Please try again in a few moments.";	// Disconnect Error Message

/* Daialog Timeout */
/* ダイアログ表示がタイムアウトするまでの時間 */
var DialogTimeOut : float = 2.0f;	// 2 seconds


/*
 * Private members
 */
 
/* ControllerID */
/* コントローラーID */
private var _controllerID : int = 0;

/* Menu Title */
/* メニュータイトル */
private var _menuTitle : String = null;

/* Menu Type */
/* 表示するメニューの種類 */
private static var TYPE_SELECT_MENU : int = 0;	// Select Menu
private static var TYPE_DIALOG : int = 1;		// Dialog
private var _menuType : int = 0;

/* Menu Item List */
/* メニュー項目のリスト */
private var _menuItemList : String[] = null;

/* Selected Menu Item Index */
/* 選択されたメニュー項目のインデックス値 */
private var _selectedIdx : int = -1;

/* Available Controller Name List */
/* 接続可能なコントローラーの名前のリスト */
private var _nameList : String[] = null;

/* Show Menu Flag */
/* メニュー表示フラグ */
private var _showMenu : boolean = false;

/* Dialog Message */
/* ダイアログに表示するメッセージ */
private var _message : String = null;

/* Disconnect Menu Item List */
/* 切断メニューに表示するメニュー項目のリスト */
private var DisconnectMenuItemList : String[] = null;


/*
 * Show Controller Menu
 * コントローラーメニューを表示する
 */
function ShowControllerMenu(controllerID : int, menuTitle : String) : boolean
{
	if(_showMenu == false) {
		/* Show controller menu. */
		/* コントローラーメニューを表示する */
		_controllerID = controllerID;
		_menuTitle = menuTitle;
		
		if(!(ZeemoteInput.IsConnected(_controllerID))) {
			/*
			 * The controller is disconnected.
			 * Show the connect menu.
			 */
			/* 
			 * 指定コントローラーIDが未接続状態の場合
			 * 接続メニューを表示する
			 */
			StartCoroutine(ConnectionMenuProcess());
		}	
		else {
			/* 
			 * The controller is connected.
			 * Show the disconnect menu.
			 */
			/*
			 * 指定コントローラーIDが接続状態の場合
			 * 切断メニューを表示する
			 */
			StartCoroutine(DisconnectionMenuProcess());
		}
		return true;
	}
	else {
		/* Now the menu is being displayed. */
		/* 現在他のメニュー表示中 */
		return false;
	}
}

/*
 * Start callback
 */
function Start() {
	ZeemoteInput.SetupPlugin();
}	

/*
 * On GUI callback
 */
function OnGUI () {

	if(!_showMenu)
		/* メニュー表示フラグがfalseなら何も表示せずにリターンする */
		return;

	switch(_menuType) {
	
	case TYPE_SELECT_MENU:
		/* Draw the select menu. */
		/* 選択メニューを描画する */
		_selectedIdx = DrawSelectMenu(_menuTitle, _menuItemList);
		break;
		
	case TYPE_DIALOG:
		/* Draw the dialog. */
		/* ダイアログを描画する */
		DrawDialog(_message);
		
		break;
		
	}
}

/*
 * Connection Menu Process Coroutine
 * 接続メニュー処理を行うコルーティン
 */
private function ConnectionMenuProcess () {

	/* Get available controllers List. */
	/* 接続可能なコントローラーの名前リストを取得する */
	ZeemoteInput.FindAvailableControllers();
	_nameList = ZeemoteInput.GetControllerNameList();
	if(_nameList == null) {
		_nameList = new String[0];
	}
	
	/* Show connect menu. */
	/* 接続メニューを表示しユーザーによる選択を待つ */
	ShowConnectMenu();
	while(_selectedIdx == -1)
		yield;
	
	/* Menu item was selected. Hide menu. */
	/* メニュー項目が選択されたので選択メニューを消す */
	HideMenuAndDialog();
	yield WaitForSeconds(0.1);

	/* Check selected menu item. */
	/* 選択されたメニュー項目を判定する */
	if(_selectedIdx >= 0 && _selectedIdx < _nameList.Length) {
		/*
		 * Selected menu item was a controller.
		 * Show the connecting dialog
		 */
		/*
		 * コントローラーが選択された場合
		 * 接続中ダイアログを表示する
		 */
		ShowDialog(DialogMessage_Connecting_prefix + _nameList[_selectedIdx]);
		yield WaitForSeconds(0.1);
		
		/* Connect to the controller. */
		/* 選択されたコントローラーへ非同期で接続する */
		var _ConnectAsync : YieldInstruction = ZeemoteInput.ConnectControllerAsync(_controllerID, _selectedIdx);
		if(_ConnectAsync != null) {
			/*
			 * Connecting.
			 * Wait for the connection process is complete.
			 */
			/*
			 * 接続処理開始成功
			 * 接続処理の完了を待つ
			 */
			yield _ConnectAsync;

			/* Check error. */
			/* エラーが発生していないか確認する */		
			if(ZeemoteInput.GetError(_controllerID) == ZeemoteInput.ERR_NONE) {
				/* 
				 * Connection success.
				 * Hide the connecting dialog.
				 */
				/*
				 * 接続成功
				 * 接続中ダイアログを消す
				 */
				HideMenuAndDialog();
				yield WaitForSeconds(0.1);
					
				/* Show the connected dialog. */
				/* 接続完了ダイアログを表示しタイムアウトを待つ */
				ShowDialog(DialogMessage_Connected_prefix + _nameList[_selectedIdx]);
				yield WaitForSeconds(DialogTimeOut);
					
				/* Hide the dialog. */
				/* ダイアログを消す */
				HideMenuAndDialog();
				yield WaitForSeconds(0.1);
					
				/* Connection process ended. */
				/* 接続処理正常終了 */
				return;
			}
		}

		/*
		 * Connect error
		 * Hide the connecting dialog.
		 */
		/*
		 * 接続エラー発生
		 * 接続中ダイアログを消す
		 */
		HideMenuAndDialog();
		yield WaitForSeconds(0.1);
			
		/* Show the error dialog. */
		/* エラーダイアログを表示しタイムアウトを待つ */
		ShowDialog(DialogMessage_ConnectError);
		yield WaitForSeconds(DialogTimeOut);
			
		/* Hide the dialog. */
		/* ダイアログを消す */
		HideMenuAndDialog();
		yield WaitForSeconds(0.1);

		/* Connection process ended with error. */
		/* 接続処理異常終了 */
		return;
	}
	else if(_selectedIdx == _nameList.Length) {
		/*
		 * Selected menu item was the cancel.
		 * Hide the menu.
		 */
		/*
		 * 選択メニューでキャンセルが選択された場合
		 * 選択メニューを消す
		 */
		HideMenuAndDialog();
		yield WaitForSeconds(0.1);
		
		/* Connection process canceled. */
		/* 接続処理キャンセル */
		return;
	}
}

/*
 * Disconnection Menu Process Coroutine
 * 切断メニュー処理を行うコルーティン
 */
private function DisconnectionMenuProcess () {

	/* Show disconnect menu. */
	/* 切断メニュー表示しユーザーによる選択を待つ */
	ShowDisconnectMenu();
	while(_selectedIdx == -1)
		yield;

	/* Hide the menu. */
	/* メニューを消す */
	HideMenuAndDialog();
	yield WaitForSeconds(0.1);

	/* Check selected menu item. */
	/* 選択されたメニュー項目を確認する */
	if(_selectedIdx == 0) {
		/*
		 * Selected menu item was the disconnect.
		 * Show the disconnecting dialog
		 */
		/*
		 * 切断が選択された場合
		 * 切断中ダイアログを表示する
		 */
		ShowDialog(DialogMessage_Disconnecting);
		yield WaitForSeconds(0.1);
		
		/* Disconenct. */
		/* 非同期で切断処理を実行する */
		var _DisconnectAsync : YieldInstruction = ZeemoteInput.DisconnectControllerAsync(_controllerID);
		if(_DisconnectAsync != null) {
			/*
			 * Disconnecting...
			 * Wait for the disconnection process is complete.
			 */
			/*
			 * 切断処理開始成功
			 * 切断処理の完了を待つ
			 */
			yield _DisconnectAsync;

			/* Check error. */
			/* エラーが発生していないか確認する */			
			if(ZeemoteInput.GetError(_controllerID) == ZeemoteInput.ERR_NONE) {
				/*
				 * Disconnection success.
				 * Hide the disconnecting dialog.
				 */
				/*
				 * 切断成功
				 * 切断中ダイアログを消す
				 */
				HideMenuAndDialog();
				yield WaitForSeconds(0.1);
					
				/* Show the disconnected dialog. */
				/* 切断完了ダイアログを表示しタイムアウトを待つ */
				ShowDialog(DialogMessage_Disconnected);
				yield WaitForSeconds(DialogTimeOut);
					
				/* Hide the dialog. */
				/* ダイアログを消す */
				HideMenuAndDialog();
				yield WaitForSeconds(0.1);
					
				/* Disconnection process ended. */
				/* 切断処理正常終了 */
				return;
			}
		}
		
		/*
		 * Diconnect Error
		 * Hide the disconnecting dialog
		 */
		/*
		 * 切断処理でエラー発生
		 * 切断中ダイアログを消す
		 */
		HideMenuAndDialog();
		yield WaitForSeconds(0.1);
					
		/* Show the error dialog */
		/* 切断エラーダイアログを表示しタイムアウトを待つ */
		ShowDialog(DialogMessage_DisconnectError);
		yield WaitForSeconds(DialogTimeOut);
					
		/* Hide the dialog. */
		/* ダイアログを消す */
		HideMenuAndDialog();
		yield WaitForSeconds(0.1);
					
		/* Disconnection process ended with error. */
		/* 切断処理エラー終了 */
		return;
	}
	else if(_selectedIdx == 1) {
		/*
		 * Selected menu item was the cancel.
		 * Hide the menu.
		 */
		/*
		 * 選択メニューでキャンセルが選択された場合
		 * メニューを消す
		 */
		HideMenuAndDialog();
		yield WaitForSeconds(0.1);
		
		/* Disconnection process canceled. */
		/* 切断処理キャンセル */
		return;
	}
}

/*
 * Show Connect Menu
 * 接続メニュー表示
 */
private function ShowConnectMenu() {

	_selectedIdx = -1;
	_menuItemList = _nameList;
	_menuType = 0;
	_showMenu = true;

}

/*
 * Show Disconnect Menu
 * 切断メニュー表示
 */
private function ShowDisconnectMenu() {

	_selectedIdx = -1;
	/* Make disconnect menu item list. */
	if(DisconnectMenuItemList == null) {
		DisconnectMenuItemList = new String[1];
		DisconnectMenuItemList[0] = ButtonLabel_Disconnect;
	}
	_menuItemList = DisconnectMenuItemList;
	_menuType = TYPE_SELECT_MENU;
	_showMenu = true;

}

/*
 * Show Dialog
 * ダイアログ表示
 */
private function ShowDialog(message : String) {

	_message = message;
	_menuType = TYPE_DIALOG;
	_showMenu = true;

}

/*
 * Hide Menu and Dialog
 * メニューおよびダイアログ消去
 */
private function HideMenuAndDialog() {
	_showMenu = false;
}

/*
 * Draw Select Menu
 * 選択メニュー描画
 */
private function DrawSelectMenu(menuTitle : String, menuItemList : String[]) : int
{
	var _ret : int = -1;
	
	/* Calculate menu rect sizes. */
	/* メニュー表示領域を算出する */
	var _MenuWidth : int	= MenuItemWidth + 50;
	var _MenuHeight : int	= ((MenuItemHeight + 5) * (menuItemList.Length + 2)) + 10;
	var _MenuLeft : int		= Screen.width / 2 - (_MenuWidth / 2);	// Horizontal centering
	var _MenuTop : int		= Screen.height / 2 - (_MenuHeight /2);	// Vertical centering
	
	/* Begin GUI group. */
	/* GUI group 描画開始 */
	GUI.BeginGroup(Rect(_MenuLeft, _MenuTop, _MenuWidth, _MenuHeight));
	
	/* Draw menu box and menu title. */
	/* メニューボックスとメニュータイトルを描画する */
	GUI.Box(Rect (0, 0, _MenuWidth, _MenuHeight), menuTitle);
	
	/* Draw menu item lists. */
	/* 各メニュー項目を描画する */
	for(var i : int = 0; i < menuItemList.Length; i++) {
		/* Draw menu item button and check whether the button is pressed. */
		/* メニュー項目をボタンとして描画し、それが押されたかどうかをチェックする */
		if(GUI.Button(Rect (25, ((MenuItemHeight + 5) * (i + 1)) , MenuItemWidth, MenuItemHeight), menuItemList[i])) {
			/* Menu item button was pressed.*/
			/* ボタンが押されたのでメニュー項目のインデックスを保存 */
			_ret = i;
		}		
		
	}
	/* Draw the cancel button and check whether the button is pressed. */
	/* キャンセルボタンを描画し、それが押されたかどうかをチェックする */
	if(GUI.Button(Rect (25, ((MenuItemHeight + 5) * (i + 1)) , MenuItemWidth, MenuItemHeight), ButtonLabel_Cancel)) {
		/* Cancel button was pressed. */
		/* キャンセルボタンが押されたのでそのインデックスを保存 */
		_ret = i;
	}
	
	/* End GUI group. */
	/* GUI group 描画終了 */
	GUI.EndGroup();
	
	/* Return selected menu item index. */
	/* 選択されたメニュー項目のインデックスを返す (未選択の場合 -1 を返す) */
	return _ret;
}

/*
 * Draw Dialog
 * ダイアログ描画
 */
private function DrawDialog(message : String)
{
	/* Calculate dialog rect sizes. */
	/* ダイアログ表示領域を算出する */
	var _DialogWidth: int 	= MenuItemWidth + 50;
	var	_DialogHeight: int 	= MenuItemHeight + 10;
	var _DialogLeft : int	= Screen.width / 2 - (_DialogWidth / 2);	// Horizontal centering
	var _DialogTop : int	= Screen.height / 2 - (_DialogHeight /2);	// Vertical centering

	/* Modfy box style. */
	/* Boxの表示スタイルを一時的に変更する (アライメントをセンタリングに設定する) */ 
	var _backupAlignment : TextAnchor = GUI.skin.box.alignment;
	GUI.skin.box.alignment = TextAnchor.MiddleCenter;	// Set box alignment to middle center. 
	
	/* Begin GUI group. */
	/* GUI group 描画開始 */
	GUI.BeginGroup(Rect(_DialogLeft, _DialogTop, _DialogWidth, _DialogHeight));
	
	/* Draw dialog box and message. */
	/* ダイアログボックスとメッセージを描画する */
	GUI.Box(Rect (0, 0, _DialogWidth, _DialogHeight), message);
	
	/* End GUI group. */
	/* GUI group 描画終了 */
	GUI.EndGroup();
	
	/* Restore box style.*/
	/* Boxの表示スタイルを元に戻す */
	GUI.skin.box.alignment = _backupAlignment;
}
