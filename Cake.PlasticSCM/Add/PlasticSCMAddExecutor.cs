﻿using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.PlasticSCM.Runners;

namespace Cake.PlasticSCM.Add
{
    internal class PlasticSCMAddExecutor : FormattableRunner<PlasticSCMAddSettings>
    {
        /// <inheritdoc />
        public PlasticSCMAddExecutor(IFileSystem fileSystem, ICakeEnvironment environment,
            IProcessRunner processRunner, IToolLocator tools, ICakeLog log) : base(fileSystem, environment, processRunner, tools,
            "add", log)
        {
            FormatStrings.Add("format","OK\t{0}");
            FormatStrings.Add("errorformat","ERR\t{0}");
        }

        public PlasticSCMAddResult Add(PlasticSCMAddSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var arguments = GetArguments(settings);
            return Run(settings, arguments, PlasticSCMAddParser.Parse);
        }

        private ProcessArgumentBuilder GetArguments(PlasticSCMAddSettings settings)
        {
            ProcessArgumentBuilder builder = GetArguments(settings, null);

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (settings.Recursive)
                builder.Append("-R");

            if (settings.IncludeParents)
                builder.Append("--parents");

            if (settings.SkipContentCheck)
                builder.Append("--skipcontentcheck");

            if (settings.CheckoutParent)
                builder.Append("--coparent");

            if (!string.IsNullOrEmpty(settings.FileTypesFilePath))
                builder.Append("--filetypes=\"{0}\"", settings.FileTypesFilePath);

            if (settings.IgnoreFailed)
                builder.Append("--ignorefailed");

            foreach (var path in settings.Paths)
            {
                builder.AppendQuoted(path);
            }

            return builder;
        }
    }
}