<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Historial.aspx.cs" Inherits="WebGomas.Historial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Mis Pedidos — PrecisionTire</title>
    <style>

        /* =====================================================
           VARIABLES — consistentes con todo el sitio
        ===================================================== */
        :root {
            --azul:          #007BFF;
            --azul-oscuro:   #0062cc;
            --azul-claro:    #e8f0fe;
            --fondo:         #F8F9FA;
            --blanco:        #ffffff;
            --gris-suave:    #f1f3f5;
            --gris-borde:    #e9ecef;
            --gris-texto:    #6c757d;
            --negro:         #1E293B;
            --verde:         #1e8449;
            --verde-claro:   #d5f5e3;
            --verde-borde:   #a9dfbf;
            --naranja:       #d35400;
            --naranja-claro: #fef3e2;
            --naranja-borde: #f8c471;
            --sombra-card:   0 4px 24px rgba(0,0,0,0.07), 0 1px 4px rgba(0,0,0,0.04);
            --radio:         16px;
        }

        *, *::before, *::after {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: var(--fondo);
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

        /* =====================================================
           HERO BANNER
        ===================================================== */
        .hero {
            background: linear-gradient(135deg, #0f2545 0%, #1a3a6e 60%, #1565c0 100%);
            padding: 44px 40px;
            position: relative;
            overflow: hidden;
        }

        .hero::before, .hero::after {
            content: '';
            position: absolute;
            border-radius: 50%;
            background: rgba(255,255,255,0.05);
        }

        .hero::before { width: 360px; height: 360px; top: -120px; right: -60px; }
        .hero::after  { width: 220px; height: 220px; bottom: -80px; left: -40px; }

        .hero-inner {
            max-width: 1100px;
            margin: 0 auto;
            position: relative;
            z-index: 1;
        }

        .hero-titulo {
            font-size: 30px;
            font-weight: 800;
            color: var(--blanco);
            letter-spacing: -0.5px;
            margin-bottom: 6px;
        }

        .hero-subtitulo {
            font-size: 15px;
            color: rgba(255,255,255,0.65);
        }

        /* =====================================================
           CONTENIDO PRINCIPAL
        ===================================================== */
        .pagina {
            flex: 1;
            max-width: 1100px;
            width: 100%;
            margin: 0 auto;
            padding: 40px 24px 64px;
        }

        /* =====================================================
           TARJETAS DE ESTADÍSTICAS
        ===================================================== */
        .stats-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 20px;
            margin-bottom: 36px;
            animation: fadeUp 0.4s ease both;
        }

        @keyframes fadeUp {
            from { opacity: 0; transform: translateY(16px); }
            to   { opacity: 1; transform: translateY(0); }
        }

        .stat-card {
            background: var(--blanco);
            border-radius: var(--radio);
            box-shadow: var(--sombra-card);
            padding: 24px 28px;
            display: flex;
            align-items: center;
            gap: 18px;
            transition: transform 0.2s, box-shadow 0.2s;
        }

        .stat-card:hover {
            transform: translateY(-3px);
            box-shadow: 0 8px 28px rgba(0,0,0,0.10);
        }

        .stat-icono {
            width: 52px;
            height: 52px;
            border-radius: 14px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 24px;
            flex-shrink: 0;
        }

        .stat-icono.azul    { background: var(--azul-claro); }
        .stat-icono.verde   { background: var(--verde-claro); }
        .stat-icono.naranja { background: var(--naranja-claro); }

        .stat-info { flex: 1; }

        .stat-valor {
            font-size: 24px;
            font-weight: 800;
            color: var(--negro);
            letter-spacing: -0.5px;
            line-height: 1;
            margin-bottom: 4px;
        }

        .stat-valor.azul    { color: var(--azul); }
        .stat-valor.verde   { color: var(--verde); }
        .stat-valor.naranja { color: var(--naranja); }

        .stat-label {
            font-size: 13px;
            color: var(--gris-texto);
            font-weight: 500;
        }

        /* =====================================================
           CARD DE LA TABLA
        ===================================================== */
        .card-tabla {
            background: var(--blanco);
            border-radius: var(--radio);
            box-shadow: var(--sombra-card);
            overflow: hidden;
            animation: fadeUp 0.45s ease 0.1s both;
        }

        /* Cabecera de la card */
        .card-tabla-header {
            padding: 22px 28px 18px;
            border-bottom: 1px solid var(--gris-borde);
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

        .card-tabla-titulo {
            font-size: 13px;
            font-weight: 700;
            letter-spacing: 1.2px;
            text-transform: uppercase;
            color: var(--gris-texto);
        }

        .badge-total {
            background: var(--azul-claro);
            color: var(--azul);
            font-size: 12px;
            font-weight: 700;
            padding: 3px 12px;
            border-radius: 20px;
        }

        /* =====================================================
           GRIDVIEW — tabla de pedidos
        ===================================================== */
        .tabla-pedidos {
            width: 100%;
            border-collapse: collapse;
        }

        /* Encabezado */
        .tabla-pedidos thead tr th {
            padding: 12px 24px;
            font-size: 11px;
            font-weight: 700;
            letter-spacing: 1px;
            text-transform: uppercase;
            color: var(--gris-texto);
            text-align: left;
            background: var(--gris-suave);
            border-bottom: 2px solid var(--gris-borde);
        }

        .tabla-pedidos thead tr th:last-child { text-align: center; }

        /* Filas */
        .tabla-pedidos tbody tr {
            border-bottom: 1px solid var(--gris-suave);
            transition: background 0.15s;
        }

        .tabla-pedidos tbody tr:last-child { border-bottom: none; }
        .tabla-pedidos tbody tr:hover      { background: #f8faff; }

        .tabla-pedidos tbody tr td {
            padding: 18px 24px;
            font-size: 14px;
            color: var(--negro);
            vertical-align: middle;
        }

        /* Celda ID */
        .celda-id {
            font-weight: 700;
            color: var(--azul);
            font-family: 'Courier New', monospace;
            font-size: 13px;
        }

        /* Celda Fecha */
        .celda-fecha { color: var(--gris-texto); font-size: 13px; }

        /* Celda Total */
        .celda-total { font-weight: 700; }

        /* Celda Acciones */
        .celda-acciones { text-align: center; }

        /* =====================================================
           BADGES DE ESTADO
        ===================================================== */
        .badge-estado {
            display: inline-flex;
            align-items: center;
            gap: 5px;
            padding: 5px 12px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 700;
            letter-spacing: 0.3px;
        }

        .badge-estado::before {
            content: '';
            width: 6px;
            height: 6px;
            border-radius: 50%;
            background: currentColor;
            opacity: 0.7;
        }

        .estado-completado {
            background: var(--verde-claro);
            color: var(--verde);
            border: 1px solid var(--verde-borde);
        }

        .estado-procesando {
            background: var(--azul-claro);
            color: var(--azul);
            border: 1px solid #b3d4ff;
        }

        .estado-pendiente {
            background: var(--naranja-claro);
            color: var(--naranja);
            border: 1px solid var(--naranja-borde);
        }

        /* =====================================================
           BOTÓN VER DETALLES
        ===================================================== */
        .btn-ver-detalle {
            display: inline-flex;
            align-items: center;
            gap: 4px;
            color: var(--azul);
            text-decoration: none;
            font-size: 13px;
            font-weight: 600;
            padding: 6px 14px;
            border-radius: 8px;
            background: var(--azul-claro);
            transition: background 0.2s, transform 0.15s;
            border: none;
            cursor: pointer;
        }

        .btn-ver-detalle:hover {
            background: #d0e4ff;
            transform: translateY(-1px);
            text-decoration: underline;
        }

        /* =====================================================
           FOOTER
        ===================================================== */
        .page-footer {
            text-align: center;
            padding: 28px;
            color: var(--gris-texto);
            font-size: 13px;
            border-top: 1px solid var(--gris-borde);
        }

        /* =====================================================
           RESPONSIVE
        ===================================================== */
        @media (max-width: 768px) {
            .stats-grid          { grid-template-columns: 1fr; }
            .header              { padding: 16px 20px; }
            .header-nav          { display: none; }
            .hero                { padding: 32px 20px; }
            .pagina              { padding: 24px 16px 48px; }
            .hero-titulo         { font-size: 22px; }

            .tabla-pedidos thead tr th,
            .tabla-pedidos tbody tr td {
                padding: 12px 14px;
                font-size: 12px;
            }
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">

        <!-- ================================================
             HEADER
        ================================================= -->
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

        <!-- ================================================
             HERO
        ================================================= -->
        <div class="hero">
            <div class="hero-inner">
                <h1 class="hero-titulo">Mis Pedidos</h1>
                <p class="hero-subtitulo">Gestiona y revisa el historial de tus compras</p>
            </div>
        </div>

        <!-- ================================================
             CONTENIDO
        ================================================= -->
        <main class="pagina">

            <!-- ── Tarjetas de estadísticas ── -->
            <div class="stats-grid">

                <div class="stat-card">
                    <div class="stat-icono azul">💰</div>
                    <div class="stat-info">
                        <div class="stat-valor azul">
                            <asp:Label ID="lblGastoTotal" runat="server" />
                        </div>
                        <div class="stat-label">Gasto total acumulado</div>
                    </div>
                </div>

                <div class="stat-card">
                    <div class="stat-icono verde">✅</div>
                    <div class="stat-info">
                        <div class="stat-valor verde">
                            <asp:Label ID="lblPedidosCompletados" runat="server" />
                        </div>
                        <div class="stat-label">Pedidos completados</div>
                    </div>
                </div>

                <div class="stat-card">
                    <div class="stat-icono naranja">🔧</div>
                    <div class="stat-info">
                        <div class="stat-valor naranja">
                            <asp:Label ID="lblProximoServicio" runat="server" />
                        </div>
                        <div class="stat-label">Próximo servicio sugerido</div>
                    </div>
                </div>

            </div>

            <!-- ── Tabla de pedidos ── -->
            <div class="card-tabla">

                <div class="card-tabla-header">
                    <span class="card-tabla-titulo">Historial de pedidos</span>
                    <asp:Label ID="lblTotalPedidos" runat="server" CssClass="badge-total" />
                </div>

                <asp:GridView
                    ID="gvHistorial"
                    runat="server"
                    AutoGenerateColumns="false"
                    CssClass="tabla-pedidos"
                    GridLines="None"
                    OnRowDataBound="gvHistorial_RowDataBound">

                    <Columns>

                        <asp:BoundField
                            DataField="Id"
                            HeaderText="ID Pedido"
                            ItemStyle-CssClass="celda-id" />

                        <asp:BoundField
                            DataField="Fecha"
                            HeaderText="Fecha"
                            ItemStyle-CssClass="celda-fecha" />

                        <asp:BoundField
                            DataField="Total"
                            HeaderText="Total"
                            DataFormatString="{0:C2}"
                            ItemStyle-CssClass="celda-total" />

                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblEstado"
                                    runat="server"
                                    Text='<%# Eval("Estado") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <a href='<%# "DetallePedido.aspx?id=" + Eval("Id") %>'
                                   class="btn-ver-detalle">
                                    Ver detalles →
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

        </main>

        <!-- ================================================
             FOOTER
        ================================================= -->
        <footer class="page-footer">
            © 2026 PrecisionTire · Todos los derechos reservados
        </footer>

    </form>
</body>
</html>