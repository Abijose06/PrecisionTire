<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Comprobante.aspx.cs" Inherits="WebGomas.Comprobante" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <title>Comprobante</title>
    <style>
        body { font-family: 'Courier New', monospace; max-width: 400px; margin: 20px auto; font-size: 13px; }
        .centro { text-align: center; }
        .separador { border-top: 1px dashed black; margin: 8px 0; }
        .fila { display: flex; justify-content: space-between; margin: 4px 0; }
        .titulo { font-size: 16px; font-weight: bold; }
        .total { font-size: 15px; font-weight: bold; }
        .btn-imprimir { display: block; margin: 20px auto; padding: 10px 30px; font-size: 14px; cursor: pointer; }
        @media print { .btn-imprimir { display: none; } }
    </style>
    <script>
        window.onload = function () {
            if (window.location.href.indexOf("print=true") > -1) {
                window.print();
            }
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="centro">
            <p class="titulo">PRECISIONTIRE</p>
            <p>Servicios y Gomas de Calidad</p>
            <p>Santo Domingo, RD</p>
        </div>

        <div class="separador"></div>

        <div class="fila">
            <span>Factura #:</span>
            <asp:Label ID="lblIdFactura" runat="server" />
        </div>
        <div class="fila">
            <span>Fecha:</span>
            <asp:Label ID="lblFecha" runat="server" />
        </div>
        <div class="fila">
            <span>Cliente:</span>
            <asp:Label ID="lblCliente" runat="server" />
        </div>

        <div class="separador"></div>

        <div class="fila">
            <strong>Producto</strong>
            <strong>Total</strong>
        </div>

        <asp:Repeater ID="rptItems" runat="server">
            <ItemTemplate>
                <div class="fila">
                    <span><%# Eval("Cantidad") %>x <%# Eval("Marca") %> <%# Eval("Modelo") %></span>
                    <span><%# string.Format("{0:C2}", Eval("SubTotal")) %></span>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <div class="separador"></div>

        <div class="fila">
            <span>Subtotal sin ITBIS:</span>
            <asp:Label ID="lblSubtotal" runat="server" />
        </div>
        <div class="fila">
            <span>ITBIS (18%):</span>
            <asp:Label ID="lblItbis" runat="server" />
        </div>

        <div class="separador"></div>

        <div class="fila total">
            <span>TOTAL:</span>
            <asp:Label ID="lblTotal" runat="server" />
        </div>

        <div class="separador"></div>

        <div class="centro">
            <p>¡Gracias por su preferencia!</p>
            <p>Conserve este comprobante</p>
        </div>

        <button class="btn-imprimir" onclick="window.print(); return false;">🖨️ Imprimir</button>

    </form>
</body>
</html>