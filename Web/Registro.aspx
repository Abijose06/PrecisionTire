<%@ Page Language="C#" AutoEventWireup="true"
         CodeBehind="Registro.aspx.cs"
         Inherits="WebGomas.Registro" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Crear Cuenta — PrecisionTire</title>
    <style>

        :root {
            --azul:        #007BFF;
            --azul-oscuro: #0062cc;
            --azul-claro:  #e8f0fe;
            --fondo:       #F8F9FA;
            --blanco:      #ffffff;
            --gris-suave:  #f1f3f5;
            --gris-borde:  #e9ecef;
            --gris-texto:  #6c757d;
            --negro:       #1a1a2e;
            --rojo:        #e53e3e;
            --rojo-claro:  #fff5f5;
            --rojo-borde:  #fed7d7;
            --verde:       #1e8449;
            --verde-claro: #d5f5e3;
            --verde-borde: #a9dfbf;
            --radio:       14px;
            --sombra:      0 4px 32px rgba(0,0,0,0.08), 0 1px 4px rgba(0,0,0,0.04);
        }

        *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: var(--fondo);
            color: var(--negro);
            min-height: 100vh;
            display: flex;
            flex-direction: column;
        }

        /* HEADER */
        .header {
            background: var(--blanco);
            border-bottom: 1px solid var(--gris-borde);
            padding: 20px 40px;
            display: flex;
            align-items: center;
            justify-content: center;
            box-shadow: 0 1px 8px rgba(0,0,0,0.05);
        }

        .header-logo {
            font-size: 22px;
            font-weight: 800;
            color: var(--negro);
            letter-spacing: -0.5px;
            text-decoration: none;
        }

        .header-logo span { color: var(--azul); }

        /* CONTENIDO */
        .pagina {
            flex: 1;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            padding: 48px 20px;
        }

        /* CARD */
        .card-registro {
            background: var(--blanco);
            border-radius: 20px;
            box-shadow: var(--sombra);
            width: 100%;
            max-width: 480px;
            overflow: hidden;
            animation: fadeUp 0.45s ease both;
        }

        @keyframes fadeUp {
            from { opacity: 0; transform: translateY(20px); }
            to   { opacity: 1; transform: translateY(0); }
        }

        /* Cabecera */
        .card-cabecera {
            background: linear-gradient(135deg, #0f2545 0%, #1a3a6e 60%, #1565c0 100%);
            padding: 32px 36px;
            text-align: center;
            position: relative;
            overflow: hidden;
        }

        .card-cabecera::before {
            content: '';
            position: absolute;
            width: 200px;
            height: 200px;
            border-radius: 50%;
            background: rgba(255,255,255,0.05);
            top: -60px;
            right: -40px;
        }

        .cabecera-icono {
            width: 56px;
            height: 56px;
            border-radius: 50%;
            background: rgba(255,255,255,0.15);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 26px;
            margin: 0 auto 14px;
            position: relative;
            z-index: 1;
        }

        .card-cabecera h2 {
            color: var(--blanco);
            font-size: 20px;
            font-weight: 700;
            margin-bottom: 4px;
            position: relative;
            z-index: 1;
        }

        .card-cabecera p {
            color: rgba(255,255,255,0.65);
            font-size: 13px;
            position: relative;
            z-index: 1;
        }

        /* Cuerpo */
        .card-cuerpo { padding: 32px 36px; }

        /* Mensaje error */
        .mensaje-error {
            background: var(--rojo-claro);
            border: 1px solid var(--rojo-borde);
            border-radius: 10px;
            padding: 12px 16px;
            display: flex;
            align-items: center;
            gap: 10px;
            margin-bottom: 20px;
            font-size: 14px;
            color: var(--rojo);
            font-weight: 500;
        }

        /* Mensaje éxito */
        .mensaje-exito {
            background: var(--verde-claro);
            border: 1px solid var(--verde-borde);
            border-radius: 10px;
            padding: 12px 16px;
            display: flex;
            align-items: center;
            gap: 10px;
            margin-bottom: 20px;
            font-size: 14px;
            color: var(--verde);
            font-weight: 500;
        }

        /* Dos columnas */
        .fila-doble {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 14px;
        }

        /* Grupos de campo */
        .campo-grupo { margin-bottom: 16px; }

        .campo-grupo label {
            display: block;
            font-size: 12px;
            font-weight: 700;
            letter-spacing: 0.8px;
            text-transform: uppercase;
            color: var(--gris-texto);
            margin-bottom: 6px;
        }

        .campo-input {
            width: 100%;
            padding: 11px 14px;
            font-size: 14px;
            border: 2px solid var(--gris-borde);
            border-radius: var(--radio);
            color: var(--negro);
            background: var(--blanco);
            outline: none;
            transition: border-color 0.2s, box-shadow 0.2s;
            font-family: inherit;
        }

        .campo-input:focus {
            border-color: var(--azul);
            box-shadow: 0 0 0 4px rgba(0,123,255,0.10);
        }

        /* Botón registrar */
        .btn-registro {
            width: 100%;
            padding: 15px;
            font-size: 16px;
            font-weight: 700;
            letter-spacing: 0.4px;
            color: var(--blanco);
            background: var(--azul);
            border: none;
            border-radius: var(--radio);
            cursor: pointer;
            margin-top: 8px;
            transition: background 0.2s, transform 0.15s, box-shadow 0.2s;
            box-shadow: 0 4px 16px rgba(0,123,255,0.32);
        }

        .btn-registro:hover {
            background: var(--azul-oscuro);
            transform: translateY(-2px);
            box-shadow: 0 8px 24px rgba(0,123,255,0.42);
        }

        .btn-registro:active {
            transform: translateY(0);
            box-shadow: 0 2px 8px rgba(0,123,255,0.22);
        }

        /* Link login */
        .link-login {
            text-align: center;
            margin-top: 20px;
            font-size: 13px;
            color: var(--gris-texto);
        }

        .link-login a {
            color: var(--azul);
            font-weight: 600;
            text-decoration: none;
        }

        .link-login a:hover { text-decoration: underline; }

        /* Nota */
        .card-nota {
            text-align: center;
            font-size: 12px;
            color: var(--gris-texto);
            padding: 0 36px 28px;
        }

        .card-nota span { color: var(--verde); font-weight: 600; }

        /* FOOTER */
        .page-footer {
            text-align: center;
            padding: 24px;
            color: var(--gris-texto);
            font-size: 13px;
            border-top: 1px solid var(--gris-borde);
        }

        @media (max-width: 480px) {
            .card-cuerpo    { padding: 24px 20px; }
            .card-cabecera  { padding: 24px 20px; }
            .fila-doble     { grid-template-columns: 1fr; }
            .header         { padding: 16px 20px; }
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">

        <!-- HEADER -->
        <header class="header">
            <a href="Login.aspx" class="header-logo">Precision<span>Tire</span></a>
        </header>

        <main class="pagina">
            <div class="card-registro">

                <!-- Cabecera -->
                <div class="card-cabecera">
                    <div class="cabecera-icono">👤</div>
                    <h2>Crear cuenta</h2>
                    <p>Regístrate para empezar a comprar</p>
                </div>

                <!-- Cuerpo -->
                <div class="card-cuerpo">

                    <!-- Error -->
                    <asp:Panel ID="pnlError" runat="server" Visible="false">
                        <div class="mensaje-error">
                            ⚠
                            <asp:Label ID="lblError" runat="server" />
                        </div>
                    </asp:Panel>

                    <!-- Éxito -->
                    <asp:Panel ID="pnlExito" runat="server" Visible="false">
                        <div class="mensaje-exito">
                            ✓
                            <asp:Label ID="lblExito" runat="server" />
                        </div>
                    </asp:Panel>

                    <!-- Nombres y Apellidos -->
                    <div class="fila-doble">
                        <div class="campo-grupo">
                            <label>Nombres</label>
                            <asp:TextBox
                                ID="txtNombres"
                                runat="server"
                                CssClass="campo-input"
                                placeholder="Juan"
                                MaxLength="80" />
                        </div>
                        <div class="campo-grupo">
                            <label>Apellidos</label>
                            <asp:TextBox
                                ID="txtApellidos"
                                runat="server"
                                CssClass="campo-input"
                                placeholder="Pérez"
                                MaxLength="80" />
                        </div>
                    </div>

                    <!-- Correo -->
                    <div class="campo-grupo">
                        <label>Correo electrónico</label>
                        <asp:TextBox
                            ID="txtCorreo"
                            runat="server"
                            TextMode="Email"
                            CssClass="campo-input"
                            placeholder="tu@email.com"
                            MaxLength="255" />
                    </div>

                    <!-- Teléfono y Documento -->
                    <div class="fila-doble">
                        <div class="campo-grupo">
                            <label>Teléfono</label>
                            <asp:TextBox
                                ID="txtTelefono"
                                runat="server"
                                CssClass="campo-input"
                                placeholder="809-000-0000"
                                MaxLength="20" />
                        </div>
                        <div class="campo-grupo">
                            <label>Documento</label>
                            <asp:TextBox
                                ID="txtDocumento"
                                runat="server"
                                CssClass="campo-input"
                                placeholder="00000000000"
                                MaxLength="15" />
                        </div>
                    </div>

                    <!-- Dirección -->
                    <div class="campo-grupo">
                        <label>Dirección</label>
                        <asp:TextBox
                            ID="txtDireccion"
                            runat="server"
                            CssClass="campo-input"
                            placeholder="Santo Domingo, RD"
                            MaxLength="255" />
                    </div>

                    <!-- Contraseña -->
                    <div class="campo-grupo">
                        <label>Contraseña</label>
                        <asp:TextBox
                            ID="txtPassword"
                            runat="server"
                            TextMode="Password"
                            CssClass="campo-input"
                            placeholder="••••••••"
                            MaxLength="50" />
                    </div>

                    <!-- Botón registrar -->
                    <asp:Button
                        ID="btnRegistrar"
                        runat="server"
                        Text="Crear cuenta →"
                        CssClass="btn-registro"
                        OnClick="btnRegistrar_Click"
                        CausesValidation="false" />

                    <!-- Link a login -->
                    <div class="link-login">
                        ¿Ya tienes cuenta?
                        <a href="Login.aspx">Inicia sesión aquí</a>
                    </div>

                </div>

                <!-- Nota -->
                <div class="card-nota">
                    <span>🔒 Registro seguro</span> · Tus datos están protegidos
                </div>

            </div>
        </main>

        <footer class="page-footer">
            © 2026 PrecisionTire · Todos los derechos reservados
        </footer>

    </form>
</body>
</html>