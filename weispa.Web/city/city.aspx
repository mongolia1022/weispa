<%@ Page Language="C#" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="com.weispa.Web.Util" %>
<script   language="C#"   runat="server">
    private string configstr;
    void   Page_Load(object   sender,   EventArgs   e)   
    {
        WxHelper wxhelper = new WxHelper();
        SignPackage config = wxhelper.GetSignPackage(JsApiEnum.onMenuShareAppMessage | JsApiEnum.onMenuShareTimeline, false);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        configstr = serializer.Serialize(config);
    }   
  </script>
<!doctype html>
<html>
<head>
<meta charset="utf-8">
<meta content="width=device-width, initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" name="viewport" />
<title>蝶变：新桥儿女多壮志，敢叫日月换新天</title>
<link rel="stylesheet" type="text/css" href="css/style.css" />
<link rel="stylesheet" type="text/css" href="css/css.css" />
<link rel="stylesheet" type="text/css" href="iconfont/iconfont.css" />
<link rel="stylesheet" type="text/css" href="css/animate.min.css" />
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script type="text/javascript">
        wx.config(<%=configstr%>);
        wx.ready(function () {
            var share_link = 'http://www.lwgjjd.com/city/index.aspx';
            var title = '蝶变：新桥儿女多壮志，敢叫日月换新天';
            var imgurl = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/78881/pattern_141.gif';
            wx.onMenuShareAppMessage({
                title: title,
                desc: '蝶变：新桥儿女多壮志，敢叫日月换新天',
                link: share_link,
                imgUrl: imgurl
            });

            wx.onMenuShareTimeline({
                title: title,
                link: share_link,
                imgUrl: imgurl,
            });
        });
        wx.error(function (res) {
            //alert("接口验证失败，详细信息：\n" + JSON.stringify(res));
        });
    </script>
<!--JQ基础文件-->
<script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>

<!--JQ MOBILE
<script type="text/javascript" src="js/jquery.mobile-1.4.5.min.js"></script>-->

<!--IE6 PNG透明-->
<!--[if lte IE 6]>
<script src="js/DD_belatedPNG_0.0.8a.js" type="text/javascript"></script>
    <script type="text/javascript">
        DD_belatedPNG.fix('div, ul, img, li, input , a');
    </script>
<![endif]-->

<!-- Link Swiper's CSS -->
<link rel="stylesheet" href="dist/css/swiper.min.css">



</head>

<body>

<!--<audio style="display:none; height: 0" id="bg-music" preload="auto" src="music/fade.mp3" loop></audio>
<script>
document.addEventListener('DOMContentLoaded', function () {    function audioAutoPlay() {        var audio = document.getElementById('bg-music');            audio.play();        document.addEventListener("WeixinJSBridgeReady", function () {            audio.play();        }, false);    }    audioAutoPlay();});

</script>-->

<!-- Swiper -->
<div class="swiper-container">
    <div class="swiper-wrapper">
        <div class="swiper-slide city1">
        	<div class="go_back"><a href="index.html"></a></div>
        	<div class="title_red"><img src="images/title1.png" width="100%" /></div>
        	<div class="car"><img src="images/car.png" width="100%" /></div>
            <div class="nav">
            	<div class="nav_t">
                	<img src="images/nav_t.png" />
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
                <ul>
                	<div class="to_see move animated fadeIn"><img src="images/to_see.png" width="100%" /></div>
                	<li>概况</li>
                    <div class="build_details">
                    	【概况】016年12月26日，新桥街道正式成立，辖区面积30平方公里，人口46.23万人，下辖新桥、新二、上星、上寮、黄埔、万丰、沙企等7个社区。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                	<li>人文</li>
                    <div class="build_details">
                    	【人文】千年古镇，历史悠久，有明代以来传承至今的粤剧传统，有本土醒狮，有“深圳四大古墟”之一的清平古墟，有深圳唯一保存的石拱桥清朝永兴桥，有曾氏大宗祠等具有一定文物价值的古建筑50余处。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                	<li>发展</li>
                    <div class="build_details">
                    	【发展】工业重镇，有计算机、通信和其他电子设备制造业，电气机械和器材制造业，橡胶和塑料制造业等三大支柱行业；制造业企业2257家，上市企业3家，国家级高新技术企业86家
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                </ul>
            </div>
            
        </div>
        <div class="swiper-slide city2">
        
        	<div class="title_red"><img src="images/title2.png" width="100%" /></div>
        	<div class="car"><img src="images/car.png" width="100%" /></div>
            <div class="nav">
            	<div class="nav_t">
                	<img src="images/nav_t.png" />
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
                <ul>
                	<div class="to_see move animated fadeIn"><img src="images/to_see.png" width="100%" /></div>
                	<li>产值</li>
                    <div class="build_details">
                    	【产值】2017年，街道规模以上工业总产值预计509亿元，同比增长11.9%，实现社会消费品零售总额62亿元，完成固定资产投资55.46亿元。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                	<li>产业</li>
                    <div class="build_details">
                    	【产业】高起点、高标准开展产城规划设计，提升城市产业能级。完成多个工业区近23.25万平方米面积升级改造，积极推动旧工业区改造，城市高端载体迅速扩充。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                	<li>服务</li>
                    <div class="build_details">
                    	【服务】线上线下24小时无间断服务企业，走访服务重点企业、重点项目1012家次，协调解决企业诉求201宗，宝博会新桥分会场圆满落幕。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                </ul>
            </div>
            
        </div>
        <div class="swiper-slide city3">
        	<div class="title_red"><img src="images/title3.png" width="100%" /></div>
        	<div class="car"><img src="images/car.png" width="100%" /></div>
            <div class="nav">
            	<div class="nav_t">
                	<img src="images/nav_t.png" />
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
                <ul>
                	<div class="to_see move animated fadeIn"><img src="images/to_see.png" width="100%" /></div>
                	<li>安全</li>
                    <div class="build_details">
                    	【安全】安全形势总体平稳。实现对重大危险源和重点监管危化品企业分布的精准定位；打造“五分钟救援覆盖体系”；落实76个工业园区安全生产主体责任；强势执法，共排查整改隐患58684条，执法检查企业444家，执法立案220宗。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                	<li>稳定</li>
                    <div class="build_details">
                    	【稳定】社会形势稳定向好。在全区率先超额完成“两长两员”群防群治义警队伍组建，率先成立街道综治督察队；严厉打击各类违法犯罪行为，刑事治安总警情同比下降23%；率先建成社区戒毒康复服务中心。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                	<li>和谐</li>
                    <div class="build_details">
                    	【和谐】和谐法治深入人心。在公众号平台创新设立法律讲堂栏目；700余人次社区居民享受到免费法律咨询服务；开展普法宣传活动152场次；完成1个法制公园、7个社区法制长廊及4个法治教育基地宣传阵地建设。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                </ul>
                
            </div>
        </div>
        <div class="swiper-slide city4">
        	<div class="title_red"><img src="images/title4.png" width="100%" /></div>
        	<div class="car"><img src="images/car.png" width="100%" /></div>
            <div class="nav">
            	<div class="nav_t">
                	<img src="images/nav_t.png" />
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
                <ul>
                	<div class="to_see move animated fadeIn"><img src="images/to_see.png" width="100%" /></div>
                	<li>民生</li>
                    <div class="build_details">
                    	【民生】民生项目多点开花。文化艺术中心、公园等10个重大民生项目全面竣工；完成50个民生微实事项目；实现211个政务服务事项跨街道通办；街道及社区综合文化服务中心建成；开展市民文化盛宴122场次；新桥图书馆正式运营。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                	<li>教卫</li>
                    <div class="build_details">
                    	【教卫】教卫事业繁荣发展。大力推进中心片区九年一贯制学校建设、新桥小学扩建；在全区率先开通计生微信公众号和微博；全面开展“优生优育惠民工程”；积极推进“国家食品安全示范城市”创建工作。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                	<li>保障</li>
                    <div class="build_details">
                    	【保障】社会保障更加有力。举办“春风行动”等公益招聘会，提供就业岗位4.1万个。打造1万多平米的区级创业孵化基地；推动残疾人职业康复中心建设；对3个帮扶扶贫村65个扶贫项目精准施策，完成2017年脱贫任务40%以上。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                </ul>
            </div>
        </div>
        <div class="swiper-slide city5">
        	<div class="title_red"><img src="images/title5.png" width="100%" /></div>
        	<div class="car"><img src="images/car.png" width="100%" /></div>
            <div class="nav">
            	<div class="nav_t">
                	<img src="images/nav_t.png" />
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
                <ul>
                	<div class="to_see move animated fadeIn"><img src="images/to_see.png" width="100%" /></div>
                	<li>媒体</li>
                    <div class="build_details">
                    	【媒体】大力建设精彩宣传阵地。在全区率先开辟“古镇新桥”微信、微博、今日头条、腾讯企鹅号四位一体宣传新阵地，是全区第一个实现新媒体平台全面覆盖的街道；南方日报、南方都市报、深圳特区报、宝安日报等全方位宣传街道新局面。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                	<li>文化</li>
                    <div class="build_details">
                    	【文化】清平古墟、曾氏大宗祠发掘工程完工。《新桥》人文杂志正式出版发行；为市民打造“一平方米”读书角100个；开展醒狮、粤剧等本土传统文化艺术表演24场、粤剧名家展演4场，推进新桥舞麒麟申报区非物质文化遗产。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                	<li>文明</li>
                    <div class="build_details">
                    	【文明】大力弘扬社会主义核心价值。洪田火山公园爱心亭文化长廊落成，书香公园社会主义核心价值主题改造正式完工；主干道、人行天桥、城中村、农贸市场实现核心价值观装饰全覆盖；开展红马甲、爱心一族服务、公益一小时等文明提升行动。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                </ul>
            </div>
        </div>
        <div class="swiper-slide city6">
        	<div class="title_red"><img src="images/title6.png" width="100%" /></div>
        	<div class="car"><img src="images/car.png" width="100%" /></div>
            <div class="nav">
            	<div class="nav_t">
                	<img src="images/nav_t.png" />
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
                <ul>
                	<div class="to_see move animated fadeIn"><img src="images/to_see.png" width="100%" /></div>
                	<li>拆违</li>
                    <div class="build_details">
                    	【诉违】城市空间加速更新。拆除消化违建80.33万平方米，提前70天在全区率先完成“669工程”年度拆除消化违建任务。100%完成政府储备土地清理区定年度任务。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                	<li>建设</li>
                    <div class="build_details">
                    	【建设】市政配套日臻完善。完成中心路米字路口改造，打通永兴路、桥兴路、新颜路等3条断头路。大钟山城市公园年内投入使用，市民广场升级改造持续推进。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                	<li>环卫</li>
                    <div class="build_details">
                    	【环卫】环境洁净度质的突破。整治各类市容环境问题4.7万宗。对10个农贸市场、24个公厕、116个垃圾屋、23个垃圾中转站进行升级改造、硬件软件双提升。实行“以洗代扫”清洁辖区道路。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                    <li>交通</li>
                    <div class="build_details">
                    	【交通】交通秩序有序高效。开展道路隐患大排查大整治，实行“6+6+19+1”网格化交通管理模式，加强道路交通巡逻防控，查处各类违法违章行为43109宗；新桥线、洪田线等两条社区微巴线路正式投放运营。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                    <li>生态</li>
                    <div class="build_details">
                    	【生态】生态环境优化明显。瑞昌路、中心路、蚝乡路、瑞丰路、永兴路5条道路绿化提升工程完工；完成黄土裸露复绿33.8万平方米；持续推进沙福河、茅洲河等黑臭水体治理，新建雨污分流管网共计约150公里。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                </ul>
            </div>
        </div>
        <div class="swiper-slide city7">
        	<div class="title_red"><img src="images/title7.png" width="100%" /></div>
        	<div class="car"><img src="images/car.png" width="100%" /></div>
            <div class="nav">
            	<div class="nav_t">
                	<img src="images/nav_t.png" />
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
                <ul>
            		<div class="to_see move animated fadeIn"><img src="images/to_see.png" width="100%" /></div>
                	<li>党建</li>
                    <div class="build_details">
                    	【党建】夯实党建标准化基础。圆满完成社区“两委”换届选举；完成街道党群服务阵地建设及社区党群服务中心标准化建设；推动直联工作与党群服务深度融合，打造标准化直联工作室。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                	<li>廉政</li>
                    <div class="build_details">
                    	【廉政】强化党委主体责任。开展“党代表进社区”等接访、调研活动39次，接待党员群众1783人次，形成了洪田片区专项调研报告，党代表提案6条。开展“为官不为”专项治理工作，廉政纪律谈话提醒80人次。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                	<li>服务</li>
                    <div class="build_details">
                    	【服务】推进特色党建服务。在全区率先成立街道党建+360°服务联盟，构建“1+3+7+8+N”全覆盖党建地图，首创党建+360°服务U站；17个务实管用的党组织“书记项目”有序推进。
                        <div class="build_close circle iconfont">&#xe740;</div>
                    </div>
                    <div class="clear"></div>
                </ul>
            </div>
            
            <a class="red_btn2 animated pulse move" href="city2.aspx"><img src="images/btn_2018.png" width="100%" /></a>
            
        </div>
        
    </div>
    
    <!-- Add Pagination -->
    <div class="swiper-pagination "></div>
    <!-- Add Arrows -->
    <div class="swiper-button-next"></div>
    <div class="swiper-button-prev"></div>
    
</div>
<script>
    $(function () {
        $(".city1").children('.car').css('left', '83%');
        $(".city1").children('.car').css('bottom', '45%');
        $(".city1").children(".title_red").addClass('animated swing');
        $(".nav ul li").click(function () {
            $(this).css('background', 'url(images/nav_bg2.png)');
            $(this).nextAll('li').css('background', 'url(images/nav_bg.png)');
            $(this).prevAll('li').css('background', 'url(images/nav_bg.png)');
            $(this).next('.build_details').css('display', 'block');
            $(this).next('.build_details').addClass('animated bounceIn');
            $(this).next('.build_details').removeClass('bounceOut');
            $(this).nextAll('li').next('.build_details').css('display', 'none');
            $(this).prevAll('li').next('.build_details').css('display', 'none');
            $(".to_see").css('display', 'none');
        });
        $(".build_close").click(function () {
            $(this).parent('.build_details').addClass('animated bounceOut');
            $(this).parent('.build_details').removeClass('bounceIn');
            $(".nav ul li").css('background', 'url(images/nav_bg.png)');
        })
        $(".swiper-button-next").addClass('animated fadeInLeft move');
    });
    $(document).ready(function () {

        function jump(count) {
            window.setTimeout(function () {
                count--;
                if (count > 0) {
                    jump(count);
                } else {
                    $(".city1").children('.title_red').addClass('animated bounceOut');
                    function jump3(count3) {
                        window.setTimeout(function () {
                            count3--;
                            if (count3 > 0) {
                                jump3(count3);
                            } else {
                                $(".nav").css('right', '0');
                            }
                        }, 1000);
                    }
                    jump3(1);
                }
            }, 1000);
        }
        jump(4);

    });
</script>


<!-- Swiper JS -->
<script src="dist/js/swiper.min.js"></script>

<!-- Initialize Swiper -->
<script>
    var swiper = new Swiper('.swiper-container', {
        pagination: '.swiper-pagination',
        nextButton: '.swiper-button-next',
        prevButton: '.swiper-button-prev',
        paginationClickable: true,
        onSlideChangeEnd: function (swiper) {
            var spidx = swiper.activeIndex;
            $(".swiper-slide").eq(spidx).children('.car').css('left', '83%');
            $(".swiper-slide").eq(spidx).children('.car').css('bottom', '45%');

            $(".swiper-slide").eq(spidx).prevAll('.swiper-slide').children('.car').css('left', '-14%');
            $(".swiper-slide").eq(spidx).prevAll('.swiper-slide').children('.car').css('bottom', '12%');

            $(".swiper-slide").eq(spidx).nextAll('.swiper-slide').children('.car').css('left', '-14%');
            $(".swiper-slide").eq(spidx).nextAll('.swiper-slide').children('.car').css('bottom', '12%');

            $(".swiper-slide").eq(spidx).children(".title_red").removeClass('bounceOut');
            $(".swiper-slide").eq(spidx).children(".title_red").addClass('animated swing');

            $(".swiper-button-next").removeClass('animated fadeInLeft move');
            $(".nav").css('display', 'none');
            $(".nav").css('right', '-100px');

            $(".red_btn").css('display', 'none');


            function jump2(count2) {
                window.setTimeout(function () {
                    count2--;
                    if (count2 > 0) {
                        jump2(count2);
                    } else {
                        $(".swiper-slide").eq(spidx).children('.title_red').addClass('animated bounceOut');
                        $(".nav").css('display', 'block');
                        function jump3(count3) {
                            window.setTimeout(function () {
                                count3--;
                                if (count3 > 0) {
                                    jump3(count3);
                                } else {
                                    $(".nav").css('right', '0');
                                    $(".red_btn2").css('display', 'block');
                                }
                            }, 1000);
                        }
                        jump3(1);
                    }
                }, 1000);
            }
            jump2(4);

        },

        onSlideChangeStart: function (swp) {
            $(".nav").css('display', 'none');
        },



    });

</script>





</body>
</html>
