<%@ Page Language="C#" %>
<!doctype html>
<html>
<head>
<meta charset="utf-8">
<meta content="width=device-width, initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" name="viewport" />
<meta name="format-detection" content="telephone=no">
<meta name="format-detection" content="email=no">

<title>抖in广州</title>
<!--JQ基础文件-->
<script type="text/javascript" src="js/jquery-3.3.1.min.js"></script>
</head>
<body>
开始时间：<input id="start" type="text"/>-结束时间<input id="end" type="text"/>&nbsp;&nbsp;&nbsp;
<button id="btn">查询</button>
<div id="result">
<div>打开次数：<span id="opencount"></span></div>
<div>相框使用次数：<span id="usecount"></span></div>
</div>
<script> 
 function formatDate (date) {  
    var y = date.getFullYear();  
    var m = date.getMonth() + 1;  
    m = m < 10 ? '0' + m : m;  
    var d = date.getDate();  
    d = d < 10 ? ('0' + d) : d;  
    return y + '-' + m + '-' + d;  
};

$(function(){
    $("#start").val(formatDate(new Date()));
        $("#end").val(formatDate(new Date()));

    $("#btn").click(function(){
        $.ajax({
                        url:"/server/stats",
                        data:{start:$("#start").val(),end:$("#end").val()},
                        dataType:"json",
                        async:true,
                        type:"POST",
                        success:function(d){
                            $("#opencount").text(d.opencount);
                                                $("#usecount").text(d.usecount);
                        },
                        error:function(){
                        }
                    });
    });
     
})


</script>
</body>
</html>

