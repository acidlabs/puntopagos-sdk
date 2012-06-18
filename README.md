h1.PuntoPagos-SDK

This development is in a very early stage, please use it at your own risk and feel free to improve it and send Pull Requests.

h2. Installation

To install you must:
1.- Attach the reference to the dll Acid.PuntoPagos.Sdk.dll
2.- Configure this sdk by code or config file, view Configuration
3.- Use.

h2. Configuration

h3. By Code

You can configure and use this SDK to create a new instance of the class PuntoPago and calling the following order independent methods:
<pre>
	var puntoPagoSdk = new PuntoPago().SetKey("Key").SetSecretCode("SecretCode").SetEnvironment(EnvironmentForPuntoPago.Sandbox).CreateTransaction();
</pre>

h3. By Config file

If you prefer, you can configure this Sdk by config file, addings this keys of AppSetting

<pre>
	<add key ="PuntoPago-Secret" value="YOU SECRET CODE"/>
    <add key ="PuntoPago-Key" value="YOU KEY" />
    <add key="PuntoPago-Environment" value="" /><!--Values: Sandbox or Production-->
</pre>

h2. Log

By default this Sdk use Log4Net for logger, but you can set you favorite logs system, calling to SetLog method of class PuntoPago.
You need extends ILog interface located in Acid.PuntoPagos.Sdk.Interfaces.ILog
<pre>
	var puntoPagoSdk = new PuntoPago().SetLog(you instance of log);
</pre>

h2. Sample Usage

h3. Create Transaction
<pre>

	var puntoPago = new PuntoPago().CreateTransaction();
	
	var createTransactionDto = puntoPago.CreateTransaction(new CreateTransactionRequestDto(100, "123121"));
	
	//Redirect to 
	createTransactionDto.ProcessUrl
</pre>

h3. Notification Transaction
<pre>

	var puntoPago = new PuntoPago().CreateTransaction();
	//request is the WebRequest 
	var notificationTransactionDto = puntoPago.NotificationTransaction(request);
	
	//Returns if the result of transacction.
	notificationTransactionDto.IsTransactionSuccessful();
	
	//Returns response for Punto Pagos
	notificationTransactionDto.GenerateResponse();
</pre>

h3. Check Status Transaction
<pre>

	var puntoPago = new PuntoPago().CreateTransaction();
	//
	var checkTransactionRequestDto = puntoPago.CheckStatusTransaction(new CheckTransactionRequestDto(100, "Transaction Client Id", "Token of Punto Pago"));
	
	//Returns if the result of transacction.
	checkTransactionRequestDto.IsTransactionSuccessful();
</pre>

h2. Test Data

|Gateway|Payload|Expected Result|
|Transbank|Visa / 4051885600446623 / CVV: 123 / exp: any|Success|
|Transbank|Mastercard / 5186059559590568 / CVV: 123 / exp: any|Failure|

h1. TODO:

* Create Nuget Package

h2. Credits

Alejandro Labra

h1. Special Thanks

Thanks to dvinales for not suing us.