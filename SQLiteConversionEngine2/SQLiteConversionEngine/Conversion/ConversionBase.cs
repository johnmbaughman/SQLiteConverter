#region

// -----------------------------------------------------
// MIT License
// Copyright (C) 2012 John M. Baughman (jbaughmanphoto.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// -----------------------------------------------------

#endregion

using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using SQLiteConversionEngine.InformationSchema;

namespace SQLiteConversionEngine.Conversion {
	public abstract class ConversionBase {

		public ConversionBase(ConnectionStringSettings sqliteConnectionStringSettings, ConnectionStringSettings otherConnectionStringSettings) {
			Connections = new Connections {
				SQLiteConnection = sqliteConnectionStringSettings,
				OtherConnection = otherConnectionStringSettings,
			};

			SourceSchema = new Schema();
			TablesToLoad = new List<string>();
			SchemasToLoad = new List<string>();
		}

		#region Protected Properties

		public List<string> TablesToLoad { get; set; }

		public List<string> SchemasToLoad { get; set; }

		public Connections Connections { get; set; }

		public Schema SourceSchema { get; set; }

		#endregion Protected Properties

		#region Protected Variables

		protected static bool _isActive = false;
		protected static bool _cancelled = false;
		protected static Regex _keyRx = new Regex(@"([a-zA-Z_0-9]+)(\(\-\))?");
		protected static Regex _defaultValueRx = new Regex(@"\(N(\'.*\')\)");

		#endregion Protected Variables

		#region Public Abstract Methods

		/// <summary>
		/// This method takes as input the connection strings to a source database
		/// and a destination database file to create the destination derived from a
		/// schema in the source
		/// </summary>
		/// <param name="conversionHandler">The conversion handler.</param>
		/// <param name="tableSelectionHandler">The table selection handler.</param>
		/// <param name="failedViewDefinitionHandler">The failed view definition handler.</param>
		/// <param name="createTriggers">if set to <c>true</c> [create triggers].</param>
		public abstract void ConvertToDatabase(ConversionHandler conversionHandler,
											   TableSelectionHandler tableSelectionHandler,
											   FailedViewDefinitionHandler failedViewDefinitionHandler,
											   bool createTriggers);

		#endregion Public Abstract Methods

		#region Protected Abstract Methods

		/// <summary>
		/// Do the entire process of first reading the source schema, creating a corresponding
		/// destination schema.
		/// </summary>
		/// <param name="conversionHandler">The conversion handler.</param>
		/// <param name="tableSelectionHandler">The table selection handler.</param>
		/// <param name="failedViewDefinitionHandler">The failed view definition handler.</param>
		/// <param name="createTriggers">if set to <c>true</c> [create triggers].</param>
		protected abstract void ConvertSourceDatabaseToDestination(ConversionHandler conversionHandler,
																   TableSelectionHandler tableSelectionHandler,
																   FailedViewDefinitionHandler failedViewDefinitionHandler,
																   bool createTriggers);

		/// <summary>
		/// Copies the source data to destination.
		/// </summary>
		/// <param name="conversionHandler">The conversion handler.</param>
		protected abstract void CopySourceDataToDestination(ConversionHandler conversionHandler);

		/// <summary>
		/// Reads the entire source database schema.
		/// </summary>
		/// <param name="conversionHandler">The conversion handler.</param>
		/// <param name="tableSelectionHandler">The table selection handler.</param>
		protected abstract void ReadSourceSchema(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler);

		/// <summary>
		/// Creates a Table object using the source connection
		/// and the name of the table for which we need to create the schema.
		/// </summary>
		/// <param name="tableName">The name of the table for which we wants to create the table schema.</param>
		/// <param name="schemaName">Name of the schema.</param>
		/// <returns>
		/// A table schema object that represents our knowledge of the table schema
		/// </returns>
		protected abstract void CreateTableSchema(Table table);

		/// <summary>
		/// Add foreign key schema object from the specified components.
		/// </summary>
		/// <param name="table">The table to add the forgein key to.</param>
		protected abstract void CreateForeignKeySchema(Table table);

		#endregion Protected Abstract Methods

		#region Public Methods

		/// <summary>
		/// Cancels the conversion.
		/// </summary>
		public void CancelConversion() {
			_cancelled = true;
		}

		#endregion Public Methods

		#region Protected Virtual Methods

		/// <summary>
		/// Builds a SELECT query for a specific table. Needed in the process of copying rows
		/// from the source database to the destination database.
		/// </summary>
		/// <param name="ts">The table schema of the table for which we need the query.</param>
		/// <returns>The SELECT query for the table.</returns>
		protected virtual string BuildSourceTableQuery(InformationSchema.Table table) {
			StringBuilder sb = new StringBuilder();
			sb.Append("SELECT ");
			for (int i = 0; i < table.Columns.Count; i++) {
				sb.Append("[" + table.Columns[i].Name + "]");
				if (i < table.Columns.Count - 1)
					sb.Append(", ");
			}
			sb.Append(" FROM " + table.Schema + "." + "[" + table.Name + "]");
			return sb.ToString();
		}

		#endregion Protected Virtual Methods

		#region "Event" Handlers

		/// <summary>
		/// This handler is called whenever a progress is made in the conversion process.
		/// </summary>
		/// <param name="done">TRUE indicates that the entire conversion process is finished.</param>
		/// <param name="success">TRUE indicates that the current step finished successfully.</param>
		/// <param name="percent">Progress percent (0-100)</param>
		/// <param name="msg">A message that accompanies the progress.</param>
		public delegate void ConversionHandler(bool done, bool success, int percent, string msg);

		/// <summary>
		/// This handler allows the user to change which tables get converted from SQL Server
		/// to SQLite.
		/// </summary>
		/// <param name="schema">The original SQL Server DB schema</param>
		/// <returns>The same schema minus any table we don't want to convert.</returns>
		public delegate List<Table> TableSelectionHandler(List<Table> schema);

		/// <summary>
		/// This handler is called in order to handle the case when copying the SQL Server view SQL
		/// statement is not enough and the user needs to either update the view definition himself
		/// or discard the view definition from the generated SQLite database.
		/// </summary>
		/// <param name="vs">The problematic view definition</param>
		/// <returns>The updated view definition, or NULL in case the view should be discarded</returns>
		public delegate string FailedViewDefinitionHandler(View vs);

		#endregion "Event" Handlers
	}
}