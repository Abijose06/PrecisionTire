<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleProducto.aspx.cs" Inherits="WebGomas.DetalleProducto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Detalle del Producto</title>
    <style>

/* =====================================================
   RESET Y BASE
===================================================== */
*, *::before, *::after {
    box-sizing: border-box;
    margin: 0;
    padding: 0;
}

body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    background-color: #F8F9FA;
    color: #1a1a2e;
    min-height: 100vh;
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 48px 20px;
}

/* =====================================================
   NAVEGACIÓN SUPERIOR
===================================================== */
.nav-top {
    width: 100%;
    max-width: 1000px;
    margin-bottom: 28px;
}

.nav-top a {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    color: #6c757d;
    text-decoration: none;
    font-size: 14px;
    font-weight: 500;
    transition: color 0.2s;
}

.nav-top a:hover {
    color: #007BFF;
}

.nav-top a::before {
    content: '←';
    font-size: 16px;
}

/* =====================================================
   CARD PRINCIPAL
===================================================== */
.card-producto {
    background: #ffffff;
    border-radius: 20px;
    box-shadow: 0 4px 32px rgba(0, 0, 0, 0.08);
    width: 100%;
    max-width: 1000px;
    display: grid;
    grid-template-columns: 1fr 1fr;
    overflow: hidden;
    animation: fadeUp 0.5s ease both;
}

@keyframes fadeUp {
    from { opacity: 0; transform: translateY(24px); }
    to   { opacity: 1; transform: translateY(0);    }
}

/* =====================================================
   COLUMNA IZQUIERDA — IMAGEN  ← MEJORADA
===================================================== */
.col-imagen {
    background: linear-gradient(145deg, #f0f4ff 0%, #e6eeff 100%);
    display: flex;
    align-items: center;
    justify-content: center;

    /* Más padding para que la imagen respire */
    padding: 56px 40px;
    position: relative;
    overflow: hidden;
}

/* Círculo decorativo de fondo */
.col-imagen::before {
    content: '';
    position: absolute;
    width: 340px;
    height: 340px;
    border-radius: 50%;
    background: rgba(0, 123, 255, 0.05);
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
}

/* ── IMAGEN: borde redondeado, fondo blanco, sombra, padding ── */
.imagen-producto {
    position: relative;
    z-index: 1;
    width: 100%;
    max-width: 280px;
    aspect-ratio: 1;
    object-fit: contain;

    background-color: #ffffff;       /* fondo blanco detrás de la imagen   */
    border-radius: 16px;             /* bordes redondeados                  */
    padding: 12px;                   /* espacio interno para que no se pegue*/
    box-shadow:
        0 6px 15px rgba(0, 0, 0, 0.06),  /* sombra principal  */
        0 2px  8px rgba(0, 0, 0, 0.06);   /* sombra secundaria */

    transition: transform 0.4s ease, box-shadow 0.4s ease;
}

.imagen-producto:hover {
    transform: scale(1.04) translateY(-5px);
    box-shadow:
        0 16px 40px rgba(0, 0, 0, 0.13),
        0  4px 12px rgba(0, 0, 0, 0.07);
}

/* Etiqueta "Disponible" sobre la imagen */
.badge-disponible {
    position: absolute;
    top: 20px;
    left: 20px;
    background: #e8f5e9;
    color: #2e7d32;
    font-size: 12px;
    font-weight: 600;
    padding: 5px 12px;
    border-radius: 20px;
    letter-spacing: 0.5px;
    border: 1px solid #c8e6c9;
    z-index: 2;
}

/* =====================================================
   COLUMNA DERECHA — INFORMACIÓN
===================================================== */
.col-info {
    padding: 52px 48px;
    display: flex;
    flex-direction: column;
    justify-content: center;
}

/* Marca */
.info-marca {
    font-size: 12px;
    font-weight: 700;
    letter-spacing: 2.5px;
    text-transform: uppercase;
    color: #007BFF;
    margin-bottom: 10px;
}

/* Nombre */
.info-nombre {
    font-size: 30px;
    font-weight: 700;
    color: #1a1a2e;
    line-height: 1.2;
    margin-bottom: 16px;
}

/* ── PRECIO: más grande, negrita, azul ── */
.info-precio {
    font-size: 36px;              /* aumentado de 34px → 36px */
    font-weight: 800;
    color: #007BFF;
    letter-spacing: -0.5px;
    line-height: 1;

    /* Más separación hacia abajo (precio → descripción) */
    margin-bottom: 28px;
}

/* Línea separadora */
.separador {
    height: 1px;
    background: #e9ecef;
    margin: 4px 0 24px;           /* más espacio debajo del separador */
}

/* ── DESCRIPCIÓN: más separación inferior ── */
.info-descripcion {
    font-size: 14px;
    color: #6c757d;
    line-height: 1.75;

    /* Separación descripción → etiquetas */
    margin-bottom: 24px;
}

/* ── ETIQUETAS: más separación inferior ── */
.etiquetas {
    display: flex;
    flex-wrap: wrap;
    gap: 8px;

    /* Separación etiquetas → cantidad */
    margin-bottom: 32px;
}

.etiqueta {
    background: #F0F4FF;
    color: #3a5fc8;
    font-size: 12px;
    font-weight: 600;
    padding: 5px 14px;
    border-radius: 20px;
    border: 1px solid #d0dcff;
    letter-spacing: 0.3px;
    transition: background 0.2s, border-color 0.2s;
}

.etiqueta:hover {
    background: #e0e9ff;
    border-color: #b0c4ff;
}

/* =====================================================
   CONTROL DE CANTIDAD  ← MEJORADO
===================================================== */
.grupo-cantidad {
    margin-bottom: 24px;
}

.grupo-cantidad label {
    display: block;
    font-size: 12px;
    font-weight: 700;
    color: #495057;
    margin-bottom: 10px;
    letter-spacing: 1px;
    text-transform: uppercase;
}

/* ── INPUT: centrado, bordes redondeados, buen padding ── */
.input-cantidad {
    width: 96px;                  /* ancho controlado, no muy grande    */
    padding: 11px 14px;           /* padding cómodo                     */
    font-size: 17px;
    font-weight: 700;
    text-align: center;           /* número centrado                    */
    border: 2px solid #dee2e6;
    border-radius: 12px;          /* bordes redondeados                 */
    color: #1a1a2e;
    background: #fff;
    outline: none;
    transition: border-color 0.2s, box-shadow 0.2s;
    -moz-appearance: textfield;
}

.input-cantidad:focus {
    border-color: #007BFF;
    box-shadow: 0 0 0 4px rgba(0, 123, 255, 0.12);
}

/* =====================================================
   BOTÓN "AGREGAR AL CARRITO"  ← MEJORADO
===================================================== */
.btn-agregar {
    width: 100%;

    /* Padding más generoso para mayor presencia */
    padding: 17px 28px;

    font-size: 16px;
    font-weight: 700;
    letter-spacing: 0.6px;
    color: #ffffff;
    background: #007BFF;
    border: none;

    /* Bordes redondeados modernos */
    border-radius: 14px;

    cursor: pointer;
    transition:
        background   0.2s ease,
        transform    0.15s ease,
        box-shadow   0.2s ease;

    box-shadow:
        0  6px 15px rgba(0, 123, 255, 0.25),
        0  1px  3px rgba(0, 0,   0,   0.08);
}

/* ── HOVER: azul más oscuro + elevación ── */
.btn-agregar:hover {
    background: #0062cc;
    transform: translateY(-3px);
    box-shadow:
        0  8px 28px rgba(0, 123, 255, 0.45),
        0  2px  6px rgba(0, 0,   0,   0.10);
}

/* ── ACTIVE: efecto de pulsación ── */
.btn-agregar:active {
    background: #0056b3;
    transform: translateY(0px);
    box-shadow:
        0  2px  8px rgba(0, 123, 255, 0.30),
        0  1px  2px rgba(0, 0,   0,   0.06);
}

/* =====================================================
   PANEL DE ERROR
===================================================== */
.panel-error {
    background: #fff5f5;
    border: 1px solid #fed7d7;
    color: #c53030;
    padding: 16px 20px;
    border-radius: 12px;
    font-size: 15px;
    font-weight: 500;
    max-width: 1000px;
    width: 100%;
}

/* =====================================================
   RESPONSIVE — una sola columna en móvil
===================================================== */
@media (max-width: 700px) {
    .card-producto {
        grid-template-columns: 1fr;
    }

    .col-imagen {
        padding: 36px 24px;
        min-height: 240px;
    }

    .imagen-producto {
        max-width: 200px;
        padding: 16px;
    }

    .col-info {
        padding: 32px 24px;
    }

    .info-nombre {
        font-size: 24px;
    }

    .info-precio {
        font-size: 30px;
        margin-bottom: 22px;
    }

    .etiquetas {
        margin-bottom: 24px;
    }
}

    </style>
</head>
<body>
    <form id="form1" runat="server">

        <!-- Volver al catálogo -->
        <nav class="nav-top">
            <a href="Productos.aspx">Volver al catálogo</a>
        </nav>

        <!-- ================================================
             Panel principal: producto encontrado
        ================================================= -->
        <asp:Panel ID="pnlDetalle" runat="server" Visible="false">
            <div class="card-producto">

                <!-- Columna izquierda: imagen -->
                <div class="col-imagen">
                    <span class="badge-disponible">✓ Disponible</span>
                    <asp:Image
                        ID="imgProducto"
                        runat="server"
                        CssClass="imagen-producto" />
                </div>

                <!-- Columna derecha: información -->
                <div class="col-info">

                    <!-- Marca -->
                    <div class="info-marca">
                        <asp:Label ID="lblMarca" runat="server" />
                    </div>

                    <!-- Nombre -->
                    <div class="info-nombre">
                        <asp:Label ID="lblNombre" runat="server" />
                    </div>

                    <!-- Precio -->
                    <div class="info-precio">
                        <asp:Label ID="lblPrecio" runat="server" />
                    </div>

                    <div class="separador"></div>

                    <!-- Descripción estática -->
                    <p class="info-descripcion">
                        Neumático de alto rendimiento diseñado para ofrecer
                        máxima adherencia y durabilidad en todo tipo de carreteras.
                        Ideal para uso diario con excelente comportamiento en mojado.
                    </p>

                    <!-- Etiquetas de características -->
                    <div class="etiquetas">
                        <span class="etiqueta">205/55 R16</span>
                        <span class="etiqueta">Turismo</span>
                        <span class="etiqueta">4 estaciones</span>
                        <span class="etiqueta">Bajo ruido</span>
                    </div>

                    <!-- Cantidad -->
                    <div class="grupo-cantidad">
                        <label for="txtCantidad">Cantidad</label>
                        <asp:TextBox
                            ID="txtCantidad"
                            runat="server"
                            Text="1"
                            MaxLength="3"
                            CssClass="input-cantidad" />
                    </div>

                    <!-- Botón agregar -->
                    <asp:Button
                        ID="btnAgregarCarrito"
                        runat="server"
                        Text="🛒  Agregar al carrito"
                        CssClass="btn-agregar"
                        OnClick="btnAgregarCarrito_Click" />

                </div>
            </div>
        </asp:Panel>

        <!-- ================================================
             Panel de error: producto no encontrado
        ================================================= -->
        <asp:Panel ID="pnlError" runat="server" Visible="false">
            <div class="panel-error">
                ⚠ Producto no encontrado. El id indicado no existe.
            </div>
        </asp:Panel>

    </form>
</body>
</html>
