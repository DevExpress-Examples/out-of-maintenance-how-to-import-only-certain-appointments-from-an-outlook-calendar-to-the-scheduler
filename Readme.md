# How to import only certain appointments from an Outlook calendar to the Scheduler


<p>This sample demonstrates how to filter outlook objects before they are imported into the Scheduler. In this example, we are loading appointments for the current week only. </p><p>The following appointments are imported:</p><p>- all simple non-recurring appointments in a specified week interval;<br />
- all series of recurring appointments, if any of these appointments occur during a specified week. (Note that the entire series are imported, including simple occurrences and exceptions).</p>


<h3>Description</h3>

<p>To import data, use the <strong>SetCalendarProvider</strong> and <strong>Import</strong> methods of the <strong>OutlookImport</strong> class.<br />
To get a list of appointments for exchange, it&#39;s necessary to create the <strong>OutlookCalendarProvider</strong> class descendant and pass it to the<strong> OutlookImport.SetCalendarProvider</strong> method. This descendant should override the <strong>PrepareItemsForExchange</strong> method, getting the <strong>TimeInterval</strong> object as a parameter, to filter the required appointments.</p>

<br/>


