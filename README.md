<h1>PuntoPagos-SDK</h1>

This development is in a very early stage, please use it at your own risk and feel free to improve it and send Pull Requests.

<h2>Installation</h2>

To install you must: <br />
<ul>
<li>Install from <a href="http://nuget.org/packages/PuntoPagos-SDK">nuget</a> PM> Install-Package PuntoPagos-SDK</li>
<li>Configure this sdk by code or config file, view <a href="https://github.com/alabras/PuntoPagos-SDK#configuration">Configuration</a></li>
<li>Use.</li>
</ul>

<h2>Configuration</h2>

<h3>By Code</h3>

You can configure and use this SDK to create a new instance of the class PuntoPago and calling the following order independent methods:
<pre>
var puntoPagoSdk = new PuntoPago().SetKey("Key").SetSecretCode("SecretCode").SetEnvironment(EnvironmentForPuntoPago.Sandbox).CreateTransaction();
</pre>

<h3>By Config file</h3>

If you prefer, you can configure this Sdk by config file, addings this keys of AppSetting

<pre>
    &lt;add key ="PuntoPago-Secret" value="YOU SECRET CODE" /&gt;
    &lt;add key ="PuntoPago-Key" value="YOU KEY" /&gt;
    &lt;add key="PuntoPago-Environment" value="" /&gt;&lt;!--Values: Sandbox or Production--/&gt;
</pre>

<h2>Log</h2>

By default this Sdk use Log4Net for logger, but you can set you favorite logs system, calling to SetLog method of class PuntoPago.
You need extends ILog interface located in Acid.PuntoPagos.Sdk.Interfaces.ILog
<pre>
var puntoPagoSdk = new PuntoPago().SetLog(you instance of log);
</pre>

<h2>Sample Usage</h2>

<h3>Create Transaction</h3>
<pre>

	var puntoPago = new PuntoPago().CreateTransaction();
	
	var createTransactionDto = puntoPago.CreateTransaction(new CreateTransactionRequestDto(100, "123121"));
	
	//Redirect to 
	createTransactionDto.ProcessUrl
</pre>

<h3>Notification Transaction</h3>
<pre>

	var puntoPago = new PuntoPago().CreateTransaction();
	//request is the WebRequest 
	var notificationTransactionDto = puntoPago.NotificationTransaction(request);
	
	//Returns if the result of transacction.
	notificationTransactionDto.IsTransactionSuccessful();
	
	//Returns response for Punto Pagos
	notificationTransactionDto.GenerateResponse();
</pre>

<h3>Check Status Transaction</h3>
<pre>

	var puntoPago = new PuntoPago().CreateTransaction();
	//
	var checkTransactionRequestDto = puntoPago.CheckStatusTransaction(new CheckTransactionRequestDto(100, "Transaction Client Id", "Token of Punto Pago"));
	
	//Returns if the result of transacction.
	checkTransactionRequestDto.IsTransactionSuccessful();
</pre>

<h2>Test Data</h2>
<table>
<tbody><tr>
<td>Gateway</td>
<td>Payload</td>
<td>Expected Result</td>
</tr>
<tr>
<td>Transbank</td>
<td>Visa / 4051885600446623 / CVV: 123 / exp: any</td>
<td>Success</td>
</tr>
<tr>
<td>Transbank</td>
<td>Mastercard / 5186059559590568 / CVV: 123 / exp: any</td>
<td>Failure</td>
</tr>
</tbody></table>

<h1>TODO</h1>
<ul>
<li>Create Wiki</li>
</ul>

<h2>Credits</h2>

Alejandro Labra

<h1>Special Thanks</h1>

Thanks to dvinales for not suing us.