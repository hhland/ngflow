using System;
using System.Web.Services.Description;
using System.IO;
using System.Net;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;

namespace BP.DA
{
    /// <summary>
    ///  Calling webservices.
    /// </summary>
    public class DynamicWebService
    {
        /// <summary>
        ///  Calling webservices.
        /// </summary>
        private DynamicWebService()
        {

        }
        /// <summary>
        ///  Dynamic Invocation web Service 
        /// </summary>
        /// <param name="url"> Link string </param>
        /// <param name="methodname"> Method name </param>
        /// <param name="args"> Parameters </param>
        /// <returns></returns>
        public static object InvokeWebService(string url, string methodName, object[] args)
        {
            return DynamicWebService.InvokeWebService(url, null, methodName, args);
        }
        private static CookieContainer container = new CookieContainer();
        /// <summary>
        ///  Dynamic Invocation web Service 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="classname"></param>
        /// <param name="methodname"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object InvokeWebService(string url, string className, string methodName, object[] args)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if ((className == null) || (className == ""))
            {
                className = DynamicWebService.GetWsClassName(url);
            }
            try
            {
                // Get WSDL 
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");
                ServiceDescription sd = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(@namespace);

                // Generate the client proxy class code  
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CodeDomProvider icc = CodeDomProvider.CreateProvider("CSharp");
                //CSharpCodeProvider csc = new CSharpCodeProvider();
                //ICodeCompiler icc = csc.CreateCompiler();

                // Compile the parameters set  
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");
                // Compile the proxy class  
                CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                // Generate proxy instance , And call the method  
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + className, true, true);
                object obj = Activator.CreateInstance(t);

                // Set up CookieContainer 1987raymond Add to             
                PropertyInfo property = t.GetProperty("CookieContainer");
                property.SetValue(obj, container, null);

                System.Reflection.MethodInfo mi = t.GetMethod(methodName);
                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }
    }
}

