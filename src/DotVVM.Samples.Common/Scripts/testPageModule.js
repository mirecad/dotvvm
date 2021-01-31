﻿export function init(context) {
    appendLine("testPageModule: init");
}

export function dispose(context) {
    appendLine("testPageModule: dispose");
}

export const commands = {
    noArgs: function (context) {
        appendLine("testPageModule: commands.noArgs(" + serializeArgs(arguments) + ")");
    },
    oneArg: function(context, arg1) {
        appendLine("testPageModule: commands.oneArg(" + serializeArgs(arguments) + ")");
    },
    twoArgs: function (context, arg1, arg2) {
        appendLine("testPageModule: commands.twoArgs(" + serializeArgs(arguments) + ")");
    },
    syncIncrement: function (context, value) {
        appendLine("testPageModule: commands.syncIncrement(" + serializeArgs(arguments) + ")");
        return value + 1;
    },
    callIncrementCommand: async function (context, value) {
        appendLine("testPageModule: commands.callIncrementCommand(" + serializeArgs(arguments) + ")");
        return await context.namedCommands.IncrementCommand(value);
    },
    asyncIncrement: function (context, value) {
        return new Promise((resolve, reject) => {
            appendLine("testPageModule: commands.asyncIncrement(" + serializeArgs(arguments) + ") begin");
            window.setTimeout(() => {
                appendLine("testPageModule: commands.asyncIncrement(" + serializeArgs(arguments) + ") end");
                resolve(value + 1);
            }, 1000);
        });
    },
    callSetResultCommand: function(context) {
        appendLine("testPageModule: commands.callSetResultCommand(" + serializeArgs(arguments) + ")");
        context.namedCommands.SetResultCommand(1, "test", { Test: "abc" });
    }
}

function appendLine(text) {
    document.getElementById("log").innerText += text + "\r\n";
}

function serializeArgs(args) {
    let result = "";
    for (let i = 1; i < args.length; i++) {
        if (i > 1) result += ", ";
        result += JSON.stringify(args[i]);
    }
    return result;
}
