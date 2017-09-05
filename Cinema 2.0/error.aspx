<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="Cinema_2._0.error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta content='LỊCH CHIẾU PHIM' property='og:title'/>
    <meta content='Đặt vé và tra cứu thông tin khuyến mãi, vé xem phim, phim mới, rạp phim.' property='og:description'/>
    <meta content='/img/banner.jpg' property='og:image'/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="card center center-align">
        <div class="card-content">
            <h5 class="card-content green-text center">Thật xui xẻo
                <br />
                Không thể tìm thấy trang bạn yêu cầu</h5>
            <a href="default.aspx" class="btn-flat green white-text" style="border-radius: 18px"><i class="material-icons cr small left">home</i>Quay lại Trang Chủ</a>
            <br />
        </div>
        <br />
    </div>
</asp:Content>
