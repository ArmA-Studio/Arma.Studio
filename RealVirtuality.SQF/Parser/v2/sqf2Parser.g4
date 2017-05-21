/*
 * MIT License
 * 
 * Copyright (c) 2017 Marco Silipo aka X39
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

parser grammar sqf2Parser;
options
{
    language = cs;
	tokenVocab = sqf2Lexer;
}
@header
{
	using System.Collections.Generic;
    using System.Linq;
}
@parser::members
{
	private IEnumerable<SqfDefinition> BinaryDefinitions;
	private IEnumerable<SqfDefinition> UnaryDefinitions;
	private IEnumerable<SqfDefinition> NularDefinitions;
	private IEnumerable<string> PrivateKeywords;

	public sqf2Parser(ITokenStream input, IEnumerable<SqfDefinition> definitions, IEnumerable<string> privateKeywords) : this(input)
	{
		this.BinaryDefinitions = from def in definitions where def.Kind == SqfDefinition.EKind.Binary select def;
		this.UnaryDefinitions = from def in definitions where def.Kind == SqfDefinition.EKind.Unary select def;
		this.NularDefinitions = from def in definitions where def.Kind == SqfDefinition.EKind.Nular select def;
		this.PrivateKeywords = (from str in privateKeywords select str.ToLower()).ToArray();
	}
}


sqf2: code;

code: statement (ENDOP statement?)* ;
statement: assignment | binaryexpression;
assignment: variable ASSIGNOP binaryexpression
          |	{ this.PrivateKeywords.Contains(_input.Lt(1).Text.ToLower()) }? GLOBALIDENT privatevariable ASSIGNOP binaryexpression
          ;
binaryexpression: primaryexpression ( ( { this.BinaryDefinitions.ContainsName(_input.Lt(1).Text) }? GLOBALIDENT | operator) primaryexpression )*;

primaryexpression: 
                     NUMBER
                 |   STRING1
				 |	 STRING2
                 |   CURLYOPEN code? CURLYCLOSE
                 |   roundbrackets
                 |   array
                 |   unaryexpression
                 |   nularexpression
                 |   variable
                 ;
nularexpression: { this.NularDefinitions.ContainsName(_input.Lt(1).Text) }? GLOBALIDENT;
unaryexpression: { this.UnaryDefinitions.ContainsName(_input.Lt(1).Text) }? (GLOBALIDENT | NEGATION | operator) primaryexpression;
roundbrackets:	ROUNDOPEN binaryexpression ROUNDCLOSE;
array:			EDGYOPEN ( binaryexpression ( COMMA binaryexpression )* )? EDGYCLOSE;
variable:		globalvariable | privatevariable;
globalvariable: GLOBALIDENT;
privatevariable: PRIVATEIDENT;
operator: OR | AND | EQUAL | GTOREQUAL | LTOREQUAL | GT | LT | NOTEQUAL | MUL | DIV | PLUS | MINUS;


define: DEFINE GLOBALIDENT ( (ANY | DEFINE_ESCAPE)*?)?;
macro: DEFINE ROUNDOPEN ( GLOBALIDENT (COMMA GLOBALIDENT)* )? ROUNDCLOSE GLOBALIDENT ( (ANY | DEFINE_ESCAPE)*?)?;
if: IF GLOBALIDENT;
else: ELSE;
endif: ENDIF;
include: INCLUDE (STRING1 | STRING2);