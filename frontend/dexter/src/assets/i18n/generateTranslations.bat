@Echo Off

SET token=%TranslateKey%

if "%token%"=="" goto :aPrompt

goto :aCheck

:aPrompt
echo -----
Set /P token=Enter RapidAPI Azure translate key: 
setx TranslateKey %token%
goto :aCheck

:aCheck
echo -----
echo Current Azure RapidAPI Token: %token%
echo -----

call i18n-auto-translation --apiProvider azure-rapid -k %token% -t de -f en -p %cd%\en.json
call i18n-auto-translation --apiProvider azure-rapid -k %token% -t es -f en -p %cd%\en.json
call i18n-auto-translation --apiProvider azure-rapid -k %token% -t fr -f en -p %cd%\en.json
call i18n-auto-translation --apiProvider azure-rapid -k %token% -t it -f en -p %cd%\en.json
call i18n-auto-translation --apiProvider azure-rapid -k %token% -t ru -f en -p %cd%\en.json
echo -----
pause
