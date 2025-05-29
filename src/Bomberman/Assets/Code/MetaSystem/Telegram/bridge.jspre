Module.StrToMemoryBuffer = function(message) {
	var messageStr = String(message);
    var bufferSize = lengthBytesUTF8(messageStr) + 1;
	var buffer = _malloc(bufferSize);
	stringToUTF8(messageStr, buffer, bufferSize);
	return buffer;
}