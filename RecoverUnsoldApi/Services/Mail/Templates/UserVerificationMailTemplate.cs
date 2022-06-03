namespace RecoverUnsoldApi.Services.Mail.Templates;

public static class UserVerificationMailTemplate
{
    public const string Text =
        "RecoverUnsold : Vérification de compte\nBonjour {name}.\nVeuillez utiliser le code suivant afin de vérifier votre compte\n{token}Merci.\nRecoverUnsold\nCopyright © 2022 RecoverUnsold";

    public const string Html = @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional //EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd""><html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office""><head><!--[if gte mso 9]> <xml> <o:OfficeDocumentSettings> <o:AllowPNG/> <o:PixelsPerInch>96</o:PixelsPerInch> </o:OfficeDocumentSettings> </xml><![endif]--> <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""> <meta name=""viewport"" content=""width=device-width, initial-scale=1.0""> <meta name=""x-apple-disable-message-reformatting""> <meta http-equiv=""X-UA-Compatible"" content=""IE=edge""> <title></title> <style type=""text/css""> @media only screen and (min-width: 620px){.u-row{width: 600px !important;}.u-row .u-col{vertical-align: top;}.u-row .u-col-100{width: 600px !important;}}@media (max-width: 620px){.u-row-container{max-width: 100% !important; padding-left: 0px !important; padding-right: 0px !important;}.u-row .u-col{min-width: 320px !important; max-width: 100% !important; display: block !important;}.u-row{width: calc(100% - 40px) !important;}.u-col{width: 100% !important;}.u-col>div{margin: 0 auto;}}body{margin: 0; padding: 0;}table, tr, td{vertical-align: top; border-collapse: collapse;}p{margin: 0;}.ie-container table, .mso-container table{table-layout: fixed;}*{line-height: inherit;}a[x-apple-data-detectors='true']{color: inherit !important; text-decoration: none !important;}table, td{color: #000000;}</style> <link href=""https://fonts.googleapis.com/css?family=Cabin:400,700"" rel=""stylesheet"" type=""text/css""> </head><body class=""clean-body u_body"" style=""margin: 0;padding: 0;-webkit-text-size-adjust: 100%;background-color: #f0f0f0;color: #000000""><table style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;min-width: 320px;Margin: 0 auto;background-color: #f0f0f0;width:100%"" cellpadding=""0"" cellspacing=""0""> <tbody> <tr style=""vertical-align: top""> <td style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top""> <div class=""u-row-container"" style=""padding: 0px;background-color: transparent""> <div class=""u-row"" style=""Margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #003399;""> <div style=""border-collapse: collapse;display: table;width: 100%;background-color: transparent;""> <div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;""> <div style=""width: 100% !important;""> <div style=""padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;""> <table style=""font-family:'Cabin',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0""> <tbody> <tr> <td style=""overflow-wrap:break-word;word-break:break-word;padding:19px 10px 25px;font-family:'Cabin',sans-serif;"" align=""left""> <div style=""color: #e5eaf5; line-height: 110%; text-align: center; word-wrap: break-word;""> <p style=""font-size: 14px; line-height: 110%;"">RecoverUnsold : V&eacute;rification de compte</p></div></td></tr></tbody> </table> </div></div></div></div></div></div><div class=""u-row-container"" style=""padding: 0px;background-color: transparent""> <div class=""u-row"" style=""Margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #ffffff;""> <div style=""border-collapse: collapse;display: table;width: 100%;background-color: transparent;""> <div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;""> <div style=""width: 100% !important;""> <div style=""padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;""> <table style=""font-family:'Cabin',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0""> <tbody> <tr> <td style=""overflow-wrap:break-word;word-break:break-word;padding:33px 55px;font-family:'Cabin',sans-serif;"" align=""left""> <div style=""line-height: 160%; text-align: center; word-wrap: break-word;""> <p style=""font-size: 14px; line-height: 160%; text-align: left;""><span style=""font-size: 16px; line-height: 25.6px;"">Bonjour {name}.</span><br/><span style=""font-size: 16px; line-height: 25.6px;"">Nous avons re&ccedil;u votre demande de v&eacute;rification de compte.</span><br/> <span style=""font-size: 16px; line-height: 25.6px;"">Votre code &agrave; usage unique est {token}.</span><br/><span style=""font-size: 16px; line-height: 25.6px;"">Il est valable durant 10 minutes.</span></p><p style=""font-size: 14px; line-height: 160%; text-align: left;"">&nbsp;</p><p style=""font-size: 14px; line-height: 160%; text-align: left;""><span style=""font-size: 16px; line-height: 25.6px;""><em>Si vous ne venez pas de vous inscrire ou n&rsquo;avez demand&eacute; aucun code, vous pouvez ignorer ce courriel. Un autre utilisateur a peut-&ecirc;tre indiqu&eacute; votre adresse courriel par erreur.</em><br/></span></p></div></td></tr></tbody> </table> <table style=""font-family:'Cabin',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0""> <tbody> <tr> <td style=""overflow-wrap:break-word;word-break:break-word;padding:14px 55px 32px;font-family:'Cabin',sans-serif;"" align=""left""> <div style=""line-height: 160%; text-align: center; word-wrap: break-word;""> <p style=""font-size: 14px; line-height: 160%;"">Merci</p><p style=""font-size: 14px; line-height: 160%;"">&nbsp;</p></div></td></tr></tbody> </table> </div></div></div></div></div></div><div class=""u-row-container"" style=""padding: 0px;background-color: transparent""> <div class=""u-row"" style=""Margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #003399;""> <div style=""border-collapse: collapse;display: table;width: 100%;background-color: transparent;""> <div class=""u-col u-col-100"" style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;""> <div style=""width: 100% !important;""> <div style=""padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;""> <table style=""font-family:'Cabin',sans-serif;"" role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" border=""0""> <tbody> <tr> <td style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:'Cabin',sans-serif;"" align=""left""> <div style=""color: #fafafa; line-height: 180%; text-align: center; word-wrap: break-word;""> <p style=""font-size: 14px; line-height: 180%;""><span style=""font-size: 16px; line-height: 28.8px;"">Copyright &copy;&nbsp; 2022 RecoverUnsold. Tous droits r&eacute;serv&eacute;s<br/></span></p></div></td></tr></tbody> </table> </div></div></div></div></div></div></td></tr></tbody></table></body></html>";
}