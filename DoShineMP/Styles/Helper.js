var oldIn = 0;  //用户报修列表
var oldIn1 = 0; //企业报修列表

//微信多图轮播
$(document).on('click', '.imgyulan', function () {

    var imglist = $(this).parent().find('.imgyulan');
    var imgurl = $(this).attr('src');
    var imgstr = '';
    for (var i = 0; i < imglist.length; i++) {
        if (imgstr.length == 0) {
            //console.log(imglist[i]);
            imgstr += imglist[i].attributes['src'].value;
        } else {
            imgstr += ',' + imglist[i].attributes['src'].value;
        }
    }
    wx.previewImage({
        current: imgurl,
        urls: imgstr.split(',')
    });
})


function setOldIndex() {
    setInterval(function () {
        if (oldIn != window.localStorage['IndexLi']) {
            window.location.reload();
        }
    }, 1000)
}


function setOldIndex1() {
    setInterval(function () {
        if (oldIn != window.localStorage['IndexLi1']) {
            window.location.reload();
        }
    }, 1000)
}