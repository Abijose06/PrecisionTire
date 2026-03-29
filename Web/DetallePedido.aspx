<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetallePedido.aspx.cs" Inherits="WebGomas.DetallePedido" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Detalle del Pedido — RuedaMax</title>
    <style>
        :root {
            --azul:        #007BFF;
            --azul-claro:  #e8f0fe;
            --fondo:       #F8F9FA;
            --blanco:      #ffffff;
            --gris-borde:  #e9ecef;
            --gris-texto:  #6c757d;
            --negro:       #1E293B;
            --verde:       #1e8449;
            --verde-claro: #d5f5e3;
            --naranja:     #d35400;
            --sombra:      0 4px 24px rgba(0,0,0,0.07);
            --radio:       16px;
        }

        *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: var(--fondo);
            color: var(--negro);
            min-height: 100vh;
            display: flex;
            flex-direction: column;
        }

        /* =====================================================
   HEADER
===================================================== */
.header {
    background: var(--blanco);
    border-bottom: 1px solid var(--gris-borde);
    padding: 0 40px;
    height: 64px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    position: sticky;
    top: 0;
    z-index: 100;
    box-shadow: 0 1px 8px rgba(0,0,0,0.05);
}

.header-logo {
    font-size: 20px;
    font-weight: 800;
    color: var(--negro);
    letter-spacing: -0.5px;
    text-decoration: none;
    flex-shrink: 0;
}

.header-logo span { color: var(--azul); }

.header-nav {
    display: flex;
    align-items: center;
    gap: 4px;
}

.header-nav a {
    color: var(--gris-texto);
    text-decoration: none;
    font-size: 14px;
    font-weight: 500;
    padding: 6px 14px;
    border-radius: 8px;
    transition: color 0.2s, background 0.2s;
    white-space: nowrap;
}

.header-nav a:hover  { color: var(--azul); background: var(--azul-claro); }
.header-nav a.activo { color: var(--azul); font-weight: 700; background: var(--azul-claro); }

.header-user {
    display: flex;
    align-items: center;
    flex-shrink: 0;
}

.btn-login-header {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    padding: 8px 16px;
    background: var(--azul);
    color: var(--blanco);
    text-decoration: none;
    font-size: 13px;
    font-weight: 600;
    border-radius: 10px;
    transition: background 0.2s, transform 0.15s;
    box-shadow: 0 2px 8px rgba(0,123,255,0.25);
}

.btn-login-header:hover {
    background: var(--azul-oscuro);
    transform: translateY(-1px);
}

.user-card {
    display: flex;
    align-items: center;
    gap: 10px;
    background: var(--gris-suave);
    border: 1px solid var(--gris-borde);
    border-radius: 12px;
    padding: 6px 6px 6px 12px;
}

.user-avatar {
    width: 32px;
    height: 32px;
    border-radius: 50%;
    background: var(--azul);
    color: var(--blanco);
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 14px;
    font-weight: 700;
    flex-shrink: 0;
}

.user-datos {
    display: flex;
    flex-direction: column;
    line-height: 1.3;
}

.user-saludo {
    font-size: 10px;
    font-weight: 600;
    color: var(--gris-texto);
    text-transform: uppercase;
    letter-spacing: 0.5px;
}

.user-nombre {
    font-size: 13px;
    font-weight: 700;
    color: var(--negro);
}

.btn-logout {
    width: 32px;
    height: 32px;
    border-radius: 8px;
    background: #fff5f5;
    border: 1px solid #fecaca;
    color: #e53e3e;
    display: flex;
    align-items: center;
    justify-content: center;
    text-decoration: none;
    font-size: 16px;
    transition: background 0.2s, color 0.2s, border-color 0.2s;
}

.btn-logout:hover {
    background: #e53e3e;
    color: var(--blanco);
    border-color: #e53e3e;
}

@media (max-width: 768px) {
    .header     { padding: 0 16px; }
    .header-nav { display: none; }
    .user-datos { display: none; }
    .user-card  { padding: 6px; gap: 6px; }
}

        /* CONTENIDO */
        .pagina {
            flex: 1;
            max-width: 760px;
            width: 100%;
            margin: 0 auto;
            padding: 44px 24px 64px;
            animation: fadeUp 0.4s ease both;
        }

        @keyframes fadeUp {
            from { opacity: 0; transform: translateY(16px); }
            to   { opacity: 1; transform: translateY(0); }
        }

        /* VOLVER */
        .link-volver {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            color: var(--gris-texto);
            text-decoration: none;
            font-size: 14px;
            font-weight: 500;
            margin-bottom: 28px;
            transition: color 0.2s;
        }

        .link-volver:hover { color: var(--azul); }
        .link-volver::before { content: '←'; font-size: 16px; }

        /* CARD PRINCIPAL */
        .card {
            background: var(--blanco);
            border-radius: var(--radio);
            box-shadow: var(--sombra);
            overflow: hidden;
            margin-bottom: 24px;
        }

        /* Cabecera azul oscuro */
        .card-cabecera {
            background: linear-gradient(135deg, #0f2545 0%, #1a3a6e 60%, #1565c0 100%);
            padding: 24px 32px;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

        .card-cabecera h2 {
            color: var(--blanco);
            font-size: 18px;
            font-weight: 700;
            margin-bottom: 3px;
        }

        .card-cabecera p {
            color: rgba(255,255,255,0.6);
            font-size: 13px;
        }

        .cabecera-icono {
            width: 44px;
            height: 44px;
            border-radius: 50%;
            background: rgba(255,255,255,0.12);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 20px;
        }

        /* Cuerpo de la card */
        .card-cuerpo { padding: 28px 32px; }

        /* Fila de dato */
        .dato-fila {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 14px 0;
            border-bottom: 1px solid var(--gris-borde);
            font-size: 14px;
        }

        .dato-fila:last-child { border-bottom: none; }

        .dato-label {
            color: var(--gris-texto);
            font-weight: 500;
        }

        .dato-valor {
            font-weight: 700;
            color: var(--negro);
        }

        .dato-valor.azul    { color: var(--azul); font-size: 20px; }
        .dato-valor.verde   { color: var(--verde); }
        .dato-valor.naranja { color: var(--naranja); }

        /* Badges de estado */
        .badge-estado {
            display: inline-flex;
            align-items: center;
            gap: 5px;
            padding: 5px 14px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 700;
        }

        .estado-completado { background: var(--verde-claro);  color: var(--verde);   border: 1px solid #a9dfbf; }
        .estado-procesando { background: var(--azul-claro);   color: var(--azul);    border: 1px solid #b3d4ff; }
        .estado-pendiente  { background: #fef3e2;             color: var(--naranja); border: 1px solid #f8c471; }

        /* Botón volver al historial */
        .btn-volver {
            display: block;
            width: 100%;
            padding: 15px;
            text-align: center;
            background: var(--azul);
            color: var(--blanco);
            text-decoration: none;
            font-size: 15px;
            font-weight: 700;
            border-radius: 12px;
            transition: background 0.2s, transform 0.15s;
            box-shadow: 0 4px 14px rgba(0,123,255,0.30);
        }

        .btn-volver:hover {
            background: #0062cc;
            transform: translateY(-2px);
        }

        /* Panel error */
        .panel-error {
            background: #fff5f5;
            border: 1px solid #fed7d7;
            color: #c53030;
            padding: 20px 24px;
            border-radius: var(--radio);
            font-size: 15px;
            font-weight: 500;
        }

        /* FOOTER */
        .page-footer {
            text-align: center;
            padding: 28px;
            color: var(--gris-texto);
            font-size: 13px;
            border-top: 1px solid var(--gris-borde);
        }

        @media (max-width: 600px) {
            .header { padding: 16px 20px; }
            .header-nav { display: none; }
            .card-cuerpo { padding: 20px; }
            .card-cabecera { padding: 20px; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <!-- HEADER -->
        <header class="header">

    <!-- IZQUIERDA: Logo -->
    <a href="Productos.aspx" class="header-logo">Precision<span>Tire</span></a>

    <!-- CENTRO: Navegación -->
    <nav class="header-nav">
        <a href="Productos.aspx">Catálogo</a>
        <a href="Carrito.aspx">Carrito</a>
        <a href="Historial.aspx">Mis pedidos</a>
    </nav>

    <!-- DERECHA: Usuario -->
    <div class="header-user">

        <%-- No logueado --%>
        <asp:PlaceHolder ID="phNoLogueado" runat="server">
            <a href="Login.aspx" class="btn-login-header">
                🔐 Iniciar sesión
            </a>
        </asp:PlaceHolder>

        <%-- Logueado --%>
        <asp:PlaceHolder ID="phLogueado" runat="server" Visible="false">
            <div class="user-card">
                <div class="user-avatar">
                    <asp:Label ID="lblAvatar" runat="server" />
                </div>
                <div class="user-datos">
                    <span class="user-saludo">Bienvenido</span>
                    <asp:Label ID="lblUsuario" runat="server" CssClass="user-nombre" />
                </div>
                <a href="Logout.aspx" class="btn-logout" title="Cerrar sesión">⏻</a>
            </div>
        </asp:PlaceHolder>

    </div>

</header>

        <main class="pagina">

            <a href="Historial.aspx" class="link-volver">Volver al historial</a>

            <!-- Panel: pedido encontrado -->
            <asp:Panel ID="pnlDetalle" runat="server" Visible="false">
                <div class="card">

                    <div class="card-cabecera">
                        <div>
                            <h2>Pedido <asp:Label ID="lblId" runat="server" /></h2>
                            <p>Información detallada de tu orden</p>
                        </div>
                        <div class="cabecera-icono">📦</div>
                    </div>

                    <div class="card-cuerpo">

                        <div class="dato-fila">
                            <span class="dato-label">ID del pedido</span>
                            <span class="dato-valor">
                                <asp:Label ID="lblIdDetalle" runat="server" />
                            </span>
                        </div>

                        <div class="dato-fila">
                            <span class="dato-label">Fecha</span>
                            <span class="dato-valor">
                                <asp:Label ID="lblFecha" runat="server" />
                            </span>
                        </div>

                        <div class="dato-fila">
                            <span class="dato-label">Total pagado</span>
                            <span class="dato-valor azul">
                                <asp:Label ID="lblTotal" runat="server" />
                            </span>
                        </div>

                        <div class="dato-fila">
                            <span class="dato-label">Estado</span>
                            <asp:Label ID="lblEstado" runat="server" />
                        </div>

                    </div>
                </div>

                <a href="Historial.aspx" class="btn-volver">← Volver al historial</a>
            </asp:Panel>

            <!-- Panel: pedido no encontrado -->
            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="panel-error">
                    ⚠ Pedido no encontrado. El ID indicado no existe.
                </div>
            </asp:Panel>

        </main>

        <footer class="page-footer">
            © 2026 RuedaMax · Todos los derechos reservados
        </footer>

    </form>
</body>
</html>