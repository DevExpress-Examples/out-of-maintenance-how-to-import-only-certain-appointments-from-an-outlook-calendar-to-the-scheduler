Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms
#Region "#usings"
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Native
Imports DevExpress.XtraScheduler.Outlook
Imports DevExpress.XtraScheduler.Outlook.Interop
#End Region ' #usings



Namespace ParameterizedOutlookImport
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub btnImport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImport.Click
			Dim importer As New OutlookImport(Me.schedulerStorage1)

			Dim interval As TimeInterval = CalculateCurrentWeekInterval()
			Dim provider As New ParameterizedCalendarProvider(interval)
			importer.SetCalendarProvider(provider)
			importer.Import(System.IO.Stream.Null)
		End Sub
		Private Function CalculateCurrentWeekInterval() As TimeInterval
			Dim start As DateTime = DateTimeHelper.GetStartOfWeek(DateTime.Today)
			Return New TimeInterval(start, start.AddDays(7))
		End Function
	End Class
	#Region "#outlookcalendarprovider"
	Public Class ParameterizedCalendarProvider
		Inherits OutlookCalendarProvider
		Private interval_Renamed As TimeInterval
		Public Sub New(ByVal interval As TimeInterval)
			If interval IsNot Nothing Then
				Me.interval_Renamed = interval
			Else
				Me.interval_Renamed = TimeInterval.Empty
			End If
		End Sub
		Public ReadOnly Property Interval() As TimeInterval
			Get
				Return interval_Renamed
			End Get
		End Property

		Protected Overrides Function PrepareItemsForExchange(ByVal items As _Items)  _
			As List(Of _AppointmentItem)
			 Dim result As New List(Of _AppointmentItem)()
			 If items Is Nothing Then
				Return result
			 End If

			Dim filter As String = FormatInterval(Interval)
			Dim olApt As _AppointmentItem = TryCast(items.Find(filter), _AppointmentItem)
			Do While olApt IsNot Nothing
				result.Add(olApt)
				olApt = TryCast(items.FindNext(), _AppointmentItem)
			Loop
			Return result
		End Function
		Protected Function FormatInterval(ByVal interval As TimeInterval) As String
			Return String.Format("[Start] > '{0}' AND [End] < '{1}'", _
			interval.Start.ToString("g"), interval.End.ToString("g"))
		End Function
	End Class
	#End Region ' #outlookcalendarprovider
End Namespace