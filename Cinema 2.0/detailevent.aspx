<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="detailevent.aspx.cs" Inherits="Cinema_2._0.detailevent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta content='LỊCH CHIẾU PHIM - KHUYẾN MÃI : <%=detailEvent.name %>' property='og:title' />
    <meta content='<%=detailEvent.intro %>' property='og:description' />
    <meta content='<%=detailEvent.linkPoster %>' property='og:image' />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <br />
    <div class="card" style="width: 80% !important">
        <div class="card-image">
            <img alt="<%=title + detailEvent.name %>" src="<%=detailEvent.linkPoster %>" onerror="this.src='/img/image-not-found.png'" />
        </div>
        <div class="card-content">
            <span class="card-title"><%=detailEvent.name %></span><table>
                <tr>
                    <td style="padding: 3px">Thời gian</td>
                    <td style="padding: 3px"><%=detailEvent.time%></td>
                </tr>
                <tr>
                    <td style="padding: 3px">Cụm rạp</td>
                    <td style="padding: 3px"><%=detailEvent.Producer.name %></td>
                </tr>
            </table>
            <p>Nội Dung : </p>
            <p>
                <%=detailEvent.intro %>
            </p>
        </div>
    </div>
    <br />
    <div class="card" style="width: 80% !important">
        <div class="fb-comments" data-href="<%=Request.Url.AbsoluteUri.Split('&')[0] %>" data-order-by="reverse_time" data-numposts="10" data-width="100%"></div>
    </div>
</asp:Content>
