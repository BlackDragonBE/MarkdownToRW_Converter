namespace DragonMarkdown.Utility
{
    public static class PreviewHelper
    {
        public static string css = "<!DOCTYPE html>" +
                                   "<html>" +
                                   "<head>" +
                                   "<link rel=\"stylesheet\" href=\"!!!DEFAULT_CSS_PATH!!!\">" +
                                   "<script src=\"!!!PACK_JS_PATH!!!\"></script>" +
                                   "<script>hljs.configure(); hljs.initHighlightingOnLoad();</script>" +
                                   "<style>" +
                                   "* {" +
                                   " font-size: 100%;" +
                                   " font-family: Bitter,Georgia,serif" +
                                   "}" +
                                   "/* Page Breaks */" +
                                   "/***Always insert a page break before the element***/" +
                                   ".pb_before {page-break-before: always !important;}" +
                                   "/***Always insert a page break after the element***/" +
                                   ".pb_after {page-break-after: always !important;}" +
                                   "/***Avoid page break before the element (if possible)***/" +
                                   ".pb_before_avoid {page-break-before: avoid !important;}" +
                                   "/***Avoid page break after the element (if possible)***/" +
                                   ".pb_after_avoid {page-break-after: avoid !important;}" +
                                   "/* Avoid page break inside the element (if possible) */" +
                                   ".pbi_avoid {page-break-inside: avoid !important;}" +
                                   "div.content { width: 860px }" +
                                   "img {" +
                                   "    display: block;" +
                                   "    margin-left: auto;" +
                                   "    margin-right: auto;" +
                                   "    max-width:860px;" +
                                   "}" +
                                   "em, strong{font-weight:700;color:#006837;font-style: normal;}" +
                                   "pre{white-space:pre-wrap;white-space:-moz-pre-wrap;white-space:-pre-wrap;white-space:-o-pre-wrap;font-family:'Droid Sans Mono',sans-serif!important;width:auto;overflow:visible;font-size:12px;line-height:1.333;text-align:left;border:1px solid #aaa;margin-bottom:1em;padding:0.5em}" +
                                   "code{font-size:14px;font-family:'Droid Sans Mono',monospace,sans-serif;color:#006837;}" +
                                   "a{color:#006837;}" +
                                   ".note{-webkit-border-radius:4px;-moz-border-radius:4px;border-radius:4px;font-family:'Open Sans',sans-serif;padding:14px!important;margin-top:5px;margin-bottom:17px;line-height:25.88px;font-style:normal;color:#black;background-color:#edf0d5;border:1px solid #000;}" +
                                   ".note{font-family:Helvetica,sans-serif;}" +
                                   "article .content-wrapper .note p{padding:0 0 10px;}" +
                                   "article .content-wrapper .note p:last-child{padding-bottom:0;}" +
                                   "article .content-wrapper .note em{font-family:Helvetica,sans-serif;font-weight:700;}" +
                                   "h1,h2,h3{font-size:18px;color:#006837;font-weight:700;font-family:Bitter,Georgia,serif;padding-bottom:5px;margin-top:-3px;}" +
                                   "h1{font-size:30px;}" +
                                   "h2{font-size:28px;}" +
                                   "h3{font-size:26px;}" +
                                   "@font-face{font-family:'Droid Sans Mono';font-style:normal;font-weight:400;src:local('Droid Sans Mono'),local('DroidSansMono'),url(https://themes.googleusercontent.com/static/fonts/droidsansmono/v4/ns-m2xQYezAtqh7ai59hJaH0X__W3S3MJL29bc5CWfs.woff) format('woff');}" +
                                   "@font-face{font-family:Bitter;font-style:normal;font-weight:400;src:local('Bitter-Regular'),url(https://themes.googleusercontent.com/static/fonts/bitter/v4/2PcBT6-VmYhQCus-O11S5-vvDin1pK8aKteLpeZ5c0A.woff) format('woff');}" +
                                   "@font-face{font-family:Bitter;font-style:normal;font-weight:700;src:local('Bitter-Bold'),url(https://themes.googleusercontent.com/static/fonts/bitter/v4/evC1haE-MsorTl_A7_uSGbO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');}" +
                                   "@font-face{font-family:Bitter;font-style:italic;font-weight:400;src:local('Bitter-Italic'),url(https://themes.googleusercontent.com/static/fonts/bitter/v4/eMS0tViDqryBl0EG1pqFZXYhjbSpvc47ee6xR_80Hnw.woff) format('woff');}" +
                                   "@font-face{font-family:'Work Sans';font-style:normal;font-weight:400;src:local('Work Sans'), local('WorkSans-Regular'), url(https://fonts.gstatic.com/s/worksans/v3/QGYsz_wNahGAdqQ43Rh_cqDpp_k.woff2) format('woff2');unicode-range:U+0100-024F, U+0259, U+1E00-1EFF, U+2020, U+20A0-20AB, U+20AD-20CF, U+2113, U+2C60-2C7F, U+A720-A7FF;}" +
                                   "@font-face{font-family:'Work Sans';font-style:normal;font-weight:400;src:local('Work Sans'), local('WorkSans-Regular'), url(https://fonts.gstatic.com/s/worksans/v3/QGYsz_wNahGAdqQ43Rh_fKDp.woff2) format('woff2');unicode-range:U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD;}" +
                                   "@font-face{font-family:'Work Sans';font-style:normal;font-weight:500;src:local('Work Sans Medium'), local('WorkSans-Medium'), url(https://fonts.gstatic.com/s/worksans/v3/QGYpz_wNahGAdqQ43Rh3j4P8lthN2fk.woff2) format('woff2');unicode-range:U+0100-024F, U+0259, U+1E00-1EFF, U+2020, U+20A0-20AB, U+20AD-20CF, U+2113, U+2C60-2C7F, U+A720-A7FF;}" +
                                   "@font-face{font-family:'Work Sans';font-style:normal;font-weight:500;src:local('Work Sans Medium'), local('WorkSans-Medium'), url(https://fonts.gstatic.com/s/worksans/v3/QGYpz_wNahGAdqQ43Rh3j4P8mNhN.woff2) format('woff2');unicode-range:U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD;}" +
                                   "@font-face{font-family:'Work Sans';font-style:normal;font-weight:600;src:local('Work Sans SemiBold'), local('WorkSans-SemiBold'), url(https://fonts.gstatic.com/s/worksans/v3/QGYpz_wNahGAdqQ43Rh3o4T8lthN2fk.woff2) format('woff2');unicode-range:U+0100-024F, U+0259, U+1E00-1EFF, U+2020, U+20A0-20AB, U+20AD-20CF, U+2113, U+2C60-2C7F, U+A720-A7FF;}" +
                                   "@font-face{font-family:'Work Sans';font-style:normal;font-weight:600;src:local('Work Sans SemiBold'), local('WorkSans-SemiBold'), url(https://fonts.gstatic.com/s/worksans/v3/QGYpz_wNahGAdqQ43Rh3o4T8mNhN.woff2) format('woff2');unicode-range:U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD;}" +
                                   "@font-face{font-family:'Work Sans';font-style:normal;font-weight:700;src:local('Work Sans Bold'), local('WorkSans-Bold'), url(https://fonts.gstatic.com/s/worksans/v3/QGYpz_wNahGAdqQ43Rh3x4X8lthN2fk.woff2) format('woff2');unicode-range:U+0100-024F, U+0259, U+1E00-1EFF, U+2020, U+20A0-20AB, U+20AD-20CF, U+2113, U+2C60-2C7F, U+A720-A7FF;}" +
                                   "@font-face{font-family:'Work Sans';font-style:normal;font-weight:700;src:local('Work Sans Bold'), local('WorkSans-Bold'), url(https://fonts.gstatic.com/s/worksans/v3/QGYpz_wNahGAdqQ43Rh3x4X8mNhN.woff2) format('woff2');unicode-range:U+0000-00FF, U+0131, U+0152-0153, U+02BB-02BC, U+02C6, U+02DA, U+02DC, U+2000-206F, U+2074, U+20AC, U+2122, U+2191, U+2193, U+2212, U+2215, U+FEFF, U+FFFD;}" +
                                   "@font-face{font-family:'Open Sans';font-style:normal;font-weight:300;src:local('Open Sans Light'),local('OpenSans-Light'),url(https://themes.googleusercontent.com/static/fonts/opensans/v6/DXI1ORHCpsQm3Vp6mXoaTaRDOzjiPcYnFooOUGCOsRk.woff) format('woff');}" +
                                   "@font-face{font-family:'Open Sans';font-style:normal;font-weight:400;src:local('Open Sans'),local('OpenSans'),url(https://themes.googleusercontent.com/static/fonts/opensans/v6/cJZKeOuBrn4kERxqtaUH3bO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');}" +
                                   "@font-face{font-family:'Open Sans';font-style:normal;font-weight:600;src:local('Open Sans Semibold'),local('OpenSans-Semibold'),url(https://themes.googleusercontent.com/static/fonts/opensans/v6/MTP_ySUJH_bn48VBG8sNSqRDOzjiPcYnFooOUGCOsRk.woff) format('woff');}" +
                                   "@font-face{font-family:'Open Sans';font-style:normal;font-weight:700;src:local('Open Sans Bold'),local('OpenSans-Bold'),url(https://themes.googleusercontent.com/static/fonts/opensans/v6/k3k702ZOKiLJc3WVjuplzKRDOzjiPcYnFooOUGCOsRk.woff) format('woff');}" +
                                   "@font-face{font-family:'Open Sans';font-style:normal;font-weight:800;src:local('Open Sans Extrabold'),local('OpenSans-Extrabold'),url(https://themes.googleusercontent.com/static/fonts/opensans/v6/EInbV5DfGHOiMmvb1Xr-hqRDOzjiPcYnFooOUGCOsRk.woff) format('woff');}" +
                                   "@font-face{font-family:'Open Sans';font-style:italic;font-weight:300;src:local('Open Sans Light Italic'),local('OpenSansLight-Italic'),url(https://themes.googleusercontent.com/static/fonts/opensans/v6/PRmiXeptR36kaC0GEAetxvR_54zmj3SbGZQh3vCOwvY.woff) format('woff');}" +
                                   "@font-face{font-family:'Open Sans';font-style:italic;font-weight:400;src:local('Open Sans Italic'),local('OpenSans-Italic'),url(https://themes.googleusercontent.com/static/fonts/opensans/v6/xjAJXh38I15wypJXxuGMBrrIa-7acMAeDBVuclsi6Gc.woff) format('woff');}" +
                                   "@font-face{font-family:'Open Sans';font-style:italic;font-weight:600;src:local('Open Sans Semibold Italic'),local('OpenSans-SemiboldItalic'),url(https://themes.googleusercontent.com/static/fonts/opensans/v6/PRmiXeptR36kaC0GEAetxuw_rQOTGi-AJs5XCWaKIhU.woff) format('woff');}" +
                                   "@font-face{font-family:'Open Sans';font-style:italic;font-weight:700;src:local('Open Sans Bold Italic'),local('OpenSans-BoldItalic'),url(https://themes.googleusercontent.com/static/fonts/opensans/v6/PRmiXeptR36kaC0GEAetxhbnBKKEOwRKgsHDreGcocg.woff) format('woff');}" +
                                   "@font-face{font-family:'Open Sans';font-style:italic;font-weight:800;src:local('Open Sans Extrabold Italic'),local('OpenSans-ExtraboldItalic'),url(https://themes.googleusercontent.com/static/fonts/opensans/v6/PRmiXeptR36kaC0GEAetxsBo4hlZyBvkZICS3KpNonM.woff) format('woff');}" +
                                   "@font-face{font-family:edmondsans_regularregular;src:url(https://koenig-assets.raywenderlich.com/wp-content/themes/raywenderlich/fonts/edmondsans-regular-webfont.eot);src:url(https://koenig-assets.raywenderlich.com/wp-content/themes/raywenderlich/fonts/edmondsans-regular-webfont.eot#iefix) format('embedded-opentype'),url(https://koenig-assets.raywenderlich.com/wp-content/themes/raywenderlich/fonts/edmondsans-regular-webfont.woff) format('woff'),url(https://koenig-assets.raywenderlich.com/wp-content/themes/raywenderlich/fonts/edmondsans-regular-webfont.ttf) format('truetype'),url(https://koenig-assets.raywenderlich.com/wp-content/themes/raywenderlich/fonts/edmondsans-regular-webfont.svg#edmondsans_regularregular) format('svg');font-weight:400;font-style:normal;}" +
                                   "</style>" +
                                   "</head>" +
                                   "<body>" +
                                   "<div class=\"content\">";
    }

}