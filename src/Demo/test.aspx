<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<%@ Page Language="C#" %>
<html dir="ltr" xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Untitled 1</title>

	<link rel="stylesheet" href="style1.css">
	<link rel="stylesheet" href="themes/base/jquery.ui.all.css">
	<script src="jquery-1.6.2.js"></script>
	<script src="ui/jquery.ui.core.js"></script>
	<script src="ui/jquery.ui.widget.js"></script>
	<script src="ui/jquery.ui.datepicker.js"></script>
	<script>
	$(function() {
		$( ".datepicker" ).datepicker();
	});
	</script>

</head>

<body>

<form id="form1" runat="server">

<p>Date: <input type="text" class="datepicker"></p>
<input type="text" class="datepicker">

</form>

</body>

</html>
