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

grammar sqf;
options
{
    language = cs;
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

	public sqfParser(ITokenStream input, IEnumerable<SqfDefinition> definitions) : this(input)
	{
		this.BinaryDefinitions = from def in definitions where def.Kind == SqfDefinition.EKind.Binary select def;
		this.UnaryDefinitions = from def in definitions where def.Kind == SqfDefinition.EKind.Unary select def;
		this.NularDefinitions = from def in definitions where def.Kind == SqfDefinition.EKind.Nular select def;
	}
}

sqf: code;

fragment LOWERCASE: [a-z];
fragment UPPERCASE: [A-Z];
fragment DIGIT: [0-9];
fragment LETTER: (LOWERCASE | UPPERCASE);
fragment HEXADIGIT: (DIGIT | [a-f] | [A-F]);
fragment ANY: .;
WS: [ \t\r\n]+ -> skip;
INSTRUCTION: '//@' .*? '\n';
INLINECOMMENT: '//' .*? '\n' -> skip;
BLOCKCOMMENT: '/*' .*? '*/' -> skip;
PREPROCESSOR: '#' .*? '\n' -> skip;
STRING: '"' ( ANY | '""' )*? '"' | '\'' ( ANY | '\'\'' )*? '\'';
NUMBER: ('0x' | '$') HEXADIGIT+ |  '-'? DIGIT+ ( '.' DIGIT+ )?;
IDENTIFIER: (LETTER | '_') (LETTER | DIGIT | '_')*;
OPERATOR: '||' | '&&' | '==' | '>=' | '<=' | '>' | '<' | '!=' | '*' | '/' | '>>' | '+' | '-' | ':';
NEGATION: '!';
CURLYOPEN: '{';
CURLYCLOSE: '}';
ROUNDOPEN: '(';
ROUNDCLOSE: ')';
EDGYOPEN: '[';
EDGYCLOSE: ']';


code:
        statement (';' statement?)*
    ;
statement:
             assignment
         |   binaryexpression
         ;
assignment:
			IDENTIFIER '=' binaryexpression
          |	{ _input.Lt(1).Text.Equals("private", System.StringComparison.InvariantCultureIgnoreCase) }? IDENTIFIER IDENTIFIER '=' binaryexpression
          ;
binaryexpression:
					primaryexpression ( { this.BinaryDefinitions.ContainsName(_input.Lt(1).Text) }? (IDENTIFIER | OPERATOR) primaryexpression )*
                ;

primaryexpression: 
                     NUMBER
                 |   STRING
                 |   CURLYOPEN code? CURLYCLOSE
                 |   roundbrackets
                 |   array
                 |   unaryexpression
                 |   nularexpression
                 |   variable
                 ;
nularexpression: { this.NularDefinitions.ContainsName(_input.Lt(1).Text) }? IDENTIFIER;
unaryexpression: { this.UnaryDefinitions.ContainsName(_input.Lt(1).Text) }? (IDENTIFIER | NEGATION) primaryexpression;
roundbrackets:	ROUNDOPEN binaryexpression ROUNDCLOSE;
array:			EDGYOPEN ( binaryexpression ( ',' binaryexpression )* )? EDGYCLOSE;
variable:		IDENTIFIER;