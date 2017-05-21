using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealVirtuality.SQF.Parser;
using RealVirtuality.SQF;
using System.Xml.Serialization;

namespace Sqf_Linter_Test
{
    class Program
    {
        //ToDo: Add NLog support
        //ToDo: Add ANTLR4 runtime
        enum ETarget
        {
            coco,
            antrlv1,
            antrlv2
        }
        static void Main(string[] args)
        {
            var workingDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Substring("file:///".Length));
            var target = ETarget.antrlv2;
            var enumerator = args.GetEnumerator();
            var exitAfterParse = false;
            var uriList = new List<Uri>();
            var armapath = string.Empty;
            var armaroot = new Uri(workingDir, UriKind.Absolute);
            var definitions = new List<SqfDefinition>();
            IEnumerable<string> privateTokens = new string[] { "private" };
            for (var it = enumerator.Current as string; enumerator.MoveNext(); it = enumerator.Current as string)
            {
                if (it.StartsWith("-"))
                {
                    it = it.Substring(1);
                }
                #region SWITCH
                switch (it.ToLower())
                {
                    case "?":
                    case "help":
                        Console.WriteLine($"-target <enum>\tSets the parser target to use. Possible values are: (default: {Enum.GetName(typeof(ETarget), target)}");
                        foreach (var name in Enum.GetNames(typeof(ETarget)))
                            Console.WriteLine($"\t\t{name}");
                        Console.WriteLine("-file <path>\tAdds a file to the parse list.");
                        Console.WriteLine("-armapath <arma-path>\tSets the ArmA path.");
                        Console.WriteLine("-armaroot <path>\tSets the target physical directory for the ArmA path.");
                        Console.WriteLine("-sqffile <path>\tSets the target physical path for an xml file containing the sqf definitions.");
                        Console.WriteLine($"-privatetoken <commaseparatedlist>\tSets those tokens that are viable for a 'token _localvar = value;' operation. default: {string.Join("/", privateTokens)}");
                        break;
                    case "target":
                        if (!enumerator.MoveNext())
                        {
                            Console.Error.WriteLine("Missing target value.");
                            exitAfterParse = true;
                        }
                        else
                        {
                            var names = Enum.GetNames(typeof(ETarget));
                            var targetName = (enumerator.Current as string).ToLower();
                            if (names.Contains(targetName))
                            {
                                target = (ETarget)Enum.Parse(typeof(ETarget), targetName);
                            }
                            else
                            {
                                Console.Error.WriteLine($"Invalid target value '{targetName}'.");
                                exitAfterParse = true;
                            }
                        }
                        break;
                    case "file":
                        if (!enumerator.MoveNext())
                        {
                            Console.Error.WriteLine("Missing file value.");
                            exitAfterParse = true;
                        }
                        else
                        {
                            var filepathorig = enumerator.Current as string;
                            var filepath = filepathorig.Replace('/', '\\');
                            if(filepath.Length > 1 && !filepath.StartsWith(@":\"))
                            {
                                filepath = string.Concat(workingDir, filepath);
                            }

                            if (System.IO.File.Exists(filepath))
                            {
                                uriList.Add(new Uri(filepath, UriKind.Absolute));
                            }
                            else
                            {
                                if(filepath.Equals(filepathorig))
                                {
                                    Console.Error.WriteLine($"File '{filepathorig}' is not existing.");
                                }
                                else
                                {
                                    Console.Error.WriteLine($"File '{filepathorig}' (at '{filepath}') is not existing.");
                                }
                                exitAfterParse = true;
                            }
                        }
                        break;
                    case "armapath":
                        if (!enumerator.MoveNext())
                        {
                            Console.Error.WriteLine("Missing path value.");
                            exitAfterParse = true;
                        }
                        else
                        {
                            armapath = enumerator.Current as string;
                        }
                        break;
                    case "privatetoken":
                        if (!enumerator.MoveNext())
                        {
                            Console.Error.WriteLine("Missing token list.");
                            exitAfterParse = true;
                        }
                        else
                        {
                            privateTokens = (enumerator.Current as string).Split(',');
                        }
                        break;
                    case "armaroot":
                        if (!enumerator.MoveNext())
                        {
                            Console.Error.WriteLine("Missing path value.");
                            exitAfterParse = true;
                        }
                        else
                        {
                            var dirpathorig = enumerator.Current as string;
                            var dirpath = dirpathorig.Replace('/', '\\');
                            if (dirpath.Length > 1 && !dirpath.StartsWith(@":\"))
                            {
                                dirpath = string.Concat(workingDir, dirpath);
                            }
                            if (System.IO.Directory.Exists(dirpath))
                            {
                                armaroot = new Uri(dirpath, UriKind.Absolute);
                            }
                            else
                            {
                                if (dirpath.Equals(dirpathorig))
                                {
                                    Console.Error.WriteLine($"Directory '{dirpathorig}' is not existing.");
                                }
                                else
                                {
                                    Console.Error.WriteLine($"Directory '{dirpathorig}' (at '{dirpath}') is not existing.");
                                }
                                exitAfterParse = true;
                            }
                        }
                        break;
                    case "sqffile":
                        if (!enumerator.MoveNext())
                        {
                            Console.Error.WriteLine("Missing path value.");
                            exitAfterParse = true;
                        }
                        else
                        {
                            var sqffileorig = enumerator.Current as string;
                            var sqffile = sqffileorig.Replace('/', '\\');
                            if (sqffile.Length > 1 && !sqffile.StartsWith(@":\"))
                            {
                                sqffile = string.Concat(workingDir, sqffile);
                            }
                            if (System.IO.File.Exists(sqffile))
                            {
                                using (var stream = System.IO.File.OpenRead(sqffile))
                                {
                                    var serializer = new XmlSerializer(typeof(List<SqfDefinition>));
                                    definitions = serializer.Deserialize(stream) as List<SqfDefinition>;
                                }
                            }
                            else
                            {
                                if (sqffile.Equals(sqffileorig))
                                {
                                    Console.Error.WriteLine($"File '{sqffileorig}' is not existing.");
                                }
                                else
                                {
                                    Console.Error.WriteLine($"File '{sqffileorig}' (at '{sqffile}') is not existing.");
                                }
                                exitAfterParse = true;
                            }
                        }
                        break;
                    default:
                        Console.Error.WriteLine($"Unknown instruction '{it}'.");
                        break;
                }
                #endregion
            }
            if (exitAfterParse)
            {
                return;
            }
            switch (target)
            {
                case ETarget.coco:
                    Parse_Coco(uriList);
                    break;
                case ETarget.antrlv1:
                    Parse_AntlrV1(uriList, definitions);
                    break;
                case ETarget.antrlv2:
                    Parse_AntlrV2(armapath, armaroot, uriList, definitions, privateTokens);
                    break;
            }
        }
        public static void Parse_Coco(IEnumerable<Uri> files)
        {
            Console.WriteLine("THIS TARGET IS NOT SUPPORTING ARMAPATH, ARMAROOT, PRIVATETOKEN AND SQFDEFINITIONS");
            foreach (var it in files)
            {
                var scanner = new RealVirtuality.SQF.Parser.Coco.Scanner(it.AbsolutePath);
                var parser = new RealVirtuality.SQF.Parser.Coco.Parser(scanner);
                parser.Parse();
            }
        }
        public static void Parse_AntlrV1(IEnumerable<Uri> files, IEnumerable<SqfDefinition> definitions)
        {
            Console.WriteLine("THIS TARGET IS NOT SUPPORTING ARMAPATH, PRIVATETOKEN AND ARMAROOT");
            foreach (var it in files)
            {
                using (var stream = System.IO.File.OpenRead(it.AbsolutePath))
                {
                    var inputStream = new Antlr4.Runtime.AntlrInputStream(stream);
                    var lexer = new RealVirtuality.SQF.Parser.v1.sqfLexer(inputStream);
                    var tokenStream = new Antlr4.Runtime.CommonTokenStream(lexer);
                    var p = new RealVirtuality.SQF.Parser.v1.sqfParser(tokenStream, definitions);
                    var listener = new RealVirtuality.SQF.Parser.v1.SqfListener();
                    p.AddParseListener(listener);
                    p.RemoveErrorListeners();

                    var infoList = new List<string>();
                    p.AddErrorListener(new RealVirtuality.SQF.Parser.v1.ErrorListener((recognizer, token, line, charPositionInLine, msg, ex) =>
                    {
                        switch (ex == null ? null : p.RuleNames[ex.Context.RuleIndex])
                        {

                            case "binaryexpression":
                                infoList.Add($"line {line}\tcol{charPositionInLine}\toff{token.StartIndex}\tInvalid binary expression: {msg}");
                                break;
                            case "unaryexpression":
                                infoList.Add($"line {line}\tcol{charPositionInLine}\toff{token.StartIndex}\tInvalid unary expression: {msg}");
                                break;
                            case "nularexpression":
                                infoList.Add($"line {line}\tcol{charPositionInLine}\toff{token.StartIndex}\tInvalid nular expression: {msg}");
                                break;
                            default:
                                infoList.Add($"line {line}\tcol{charPositionInLine}\toff{token.StartIndex}\t{msg}");
                                break;
                        }
                    }));
                    try
                    {
                        p.sqf();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                        while (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                            Console.Error.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }
        public static void Parse_AntlrV2(string armapath, Uri armaroot, IEnumerable<Uri> files, IEnumerable<SqfDefinition> definitions, IEnumerable<string> privateTokens)
        {
            foreach (var it in files)
            {
                using (var stream = System.IO.File.OpenRead(it.AbsolutePath))
                {
                    var inputStream = new Antlr4.Runtime.AntlrInputStream(stream);
                    var lexer = new RealVirtuality.SQF.Parser.v2.sqf2Lexer(inputStream);
                    var tokenStream = new Antlr4.Runtime.CommonTokenStream(lexer);
                    var p = new RealVirtuality.SQF.Parser.v2.sqf2Parser(tokenStream, definitions, privateTokens);
                    var listener = new RealVirtuality.SQF.Parser.v1.SqfListener();
                    p.AddParseListener(listener);
                    p.RemoveErrorListeners();

                    var infoList = new List<string>();
                    p.AddErrorListener(new RealVirtuality.SQF.Parser.v1.ErrorListener((recognizer, token, line, charPositionInLine, msg, ex) =>
                    {
                        switch (ex == null ? null : p.RuleNames[ex.Context.RuleIndex])
                        {

                            case "binaryexpression":
                                infoList.Add($"line {line}\tcol{charPositionInLine}\toff{token.StartIndex}\tInvalid binary expression: {msg}");
                                break;
                            case "unaryexpression":
                                infoList.Add($"line {line}\tcol{charPositionInLine}\toff{token.StartIndex}\tInvalid unary expression: {msg}");
                                break;
                            case "nularexpression":
                                infoList.Add($"line {line}\tcol{charPositionInLine}\toff{token.StartIndex}\tInvalid nular expression: {msg}");
                                break;
                            default:
                                infoList.Add($"line {line}\tcol{charPositionInLine}\toff{token.StartIndex}\t{msg}");
                                break;
                        }
                    }));
                    try
                    {
                        p.sqf2();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                        while (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                            Console.Error.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
