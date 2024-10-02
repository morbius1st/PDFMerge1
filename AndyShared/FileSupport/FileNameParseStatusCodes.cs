// Solution:     PDFMerge1
// Project:       Test3
// File:             FileNameParseStatusCodes.cs
// Created:      2024-09-29 (12:51 PM)

namespace AndyShared.FileSupport
{
	public enum FileNameParseStatusCodes
	{
		SC_NONE = 0,
		SC_VOID_SHTNUM = -1,
		SC_INVALID_FILENAME = -2,
		SC_INVALID_FILETYPE = -3,

		SC_PARSER_NOT_CONFIGD=-102,
		SC_PARSER_MATCH_FAILED = -103,
		SC_PARSER_COMP_EXTRACT_FAILED=-104,
	}
}