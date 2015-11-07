Option Explicit On
Option Strict On

Imports System
Imports System.Data
Imports Npgsql


''' <summary>
''' Proyecto que implementa un logn multicapa. Cada una de las clases simula una de las capas, 
''' pero por simplificar se añade todo en un único fichero y proyecto
''' </summary>
Public Class Form1
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim bd As New BaseDeDatos
        If Jugador.ComprobarLogin(TextBox1.Text, TextBox2.Text) > 0 Then
            MsgBox("Has accedido correctamente")
        Else
            MsgBox("El usuario y/o la contrseña introducidas son incorrectos")
        End If
    End Sub
End Class

''' <summary>
''' 
''' </summary>
Public Class Negocio

End Class
''' <summary>
''' Clase Jugador resumida y simplificada
''' </summary>
Public Class Jugador
    Public Property ID_JUGADOR As UInteger
    Public Property NICK_JUGADOR As String
    Public Property PASSWORD_JUGADOR As String

    Public Sub New(ByVal pId As UInteger, ByVal pNick As String, ByVal pPass As String)
        Me.ID_JUGADOR = pId
        Me.NICK_JUGADOR = pNick
        Me.PASSWORD_JUGADOR = pPass
    End Sub

    Public Overrides Function ToString() As String
        Return PASSWORD_JUGADOR
    End Function

    Public Shared Function ComprobarLogin(ByVal pNick As String, ByVal pPass As String) As Integer
        Dim bd As BaseDeDatos
        bd = New BaseDeDatos

        Return bd.ComprobarLogin(pNick, pPass)

    End Function

End Class


''' <summary>
''' Clase base de datos resumida y simplificada
''' </summary>
Public Class BaseDeDatos
    Private ConexionConBD As NpgsqlConnection
    Private Orden As NpgsqlCommand
    Private Lector As NpgsqlDataReader

    Public Function ComprobarLogin(ByVal Usuario As String, ByVal pass As String) As Integer
        Dim resultado As Integer = 0
        Dim contrase As String = ""
        'Abrir la base de datos
        Dim strConexión As String = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                                                  "LocalHost", "5432", "esports", "antonio", "esports")
        ConexionConBD = New NpgsqlConnection(strConexión)
        ConexionConBD.Open()

        'Crear una consulta
        Dim Consulta As String = "SELECT ID_JUGADOR, Password_jugador FROM Jugador WHERE Nick_jugador ='" & Usuario & "' AND Password_jugador = md5('" & pass & "')"
        Orden = New NpgsqlCommand(Consulta, ConexionConBD)

        'ExecuteReader hace la consulta y devuelve un OleDbDataReader
        Lector = Orden.ExecuteReader()
        'Llamar siempre a Read antes de acceder a los datos
        While Lector.Read() 'siguiente registro
            resultado = CInt(Lector("ID_JUGADOR"))
            contrase = CStr("Password_jugador")
        End While
        'Llamar siempre a Close una vez finalizada la lectura
        Lector.Close()
        CerrarConexion()
        Return resultado
    End Function

    Public Sub CerrarConexion()
        ' Cerrar la conexión cuando ya no sea necesaria
        If (Not Lector Is Nothing) Then
            Lector.Close()
        End If
        If (Not ConexionConBD Is Nothing) Then
            ConexionConBD.Close()
        End If
    End Sub


End Class

