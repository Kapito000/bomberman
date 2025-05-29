mergeInto(LibraryManager.library, {
	getUserId: function () {
    if (window.Telegram == null) {
      return GameInstance.Module.StrToMemoryBuffer("");
    }
    let uid = "";
    let u = window.Telegram.WebApp.initDataUnsafe.user;
    if (u != null && u.id != null) {
        uid = u.id;
      }
		return GameInstance.Module.StrToMemoryBuffer(uid);
	},
	
	getUserName: function () {
    if (window.Telegram == null) {
      return GameInstance.Module.StrToMemoryBuffer("");
    }
    let uname = "";
    let user = window.Telegram.WebApp.initDataUnsafe.user;
    if (user != null && user.username != null) {
        uname = user.username;
      }
		return GameInstance.Module.StrToMemoryBuffer(uname);
	},
	
	getUserPhotoUrl: function () {
    if (window.Telegram == null) {
      return GameInstance.Module.StrToMemoryBuffer("");
    }
    let avatar = "";
    let user = window.Telegram.WebApp.initDataUnsafe.user;
    if (user != null && user.photo_url != null) {
        avatar = user.photo_url;
      }
		return GameInstance.Module.StrToMemoryBuffer(avatar);
	},
	
	getInitData: function () {
    if (window.Telegram == null) {
      return GameInstance.Module.StrToMemoryBuffer("");
    }
    let data = window.Telegram.WebApp.initData;
		return GameInstance.Module.StrToMemoryBuffer(data);
	}
});
