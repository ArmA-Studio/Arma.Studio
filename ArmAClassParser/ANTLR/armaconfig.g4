/*
 * MIT License
 * 
 * Copyright (c) 2017 Marco Silipo
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

grammar armaconfig;
options
{
    language = cs;
}
@header
{
	using System.Collections.Generic;
    using System.Linq;
}

armaconfig: class*;

fragment LOWERCASE: [a-z];
fragment UPPERCASE: [A-Z];
fragment DIGIT: [0-9];
fragment LETTER: (LOWERCASE | UPPERCASE);
fragment ALPHANUMERIC: LETTER | DIGIT | '_';
fragment HEXADIGIT: (DIGIT | [a-f] | [A-F]);
fragment ANY: .;
WS: [ \t\r\n]+ -> skip;
INSTRUCTION: '//@' .*? '\n';
INLINECOMMENT: '//' .*? '\n' -> skip;
BLOCKCOMMENT: '/*' .*? '*/' -> skip;
PREPROCESSOR: '#' .*? '\n' -> skip;

CLASS: [cC][lL][aA][sS][sS];

LOCALIZATION: '$'ALPHANUMERIC+;
IDENT: ALPHANUMERIC+;
fragment DQSTRING: ('"'ANY*?'"')+;
fragment SQSTRING: ('\''ANY*?'\'')+;
STRING: DQSTRING | SQSTRING;



class: CLASS IDENT ( ':' IDENT )? ('{' (class | field)* '}')? ';';

field: IDENT (('[' ']' '=' array) | ('=' element)) ';';

array: '{' ((element | array) (',' (element | array))*)? '}';

element: (LOCALIZATION | IDENT | STRING | ANY)*;