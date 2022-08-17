using System.Collections.Generic;
using Naninovel;
using Naninovel.UI;
using Naninovel.Commands;
using UnityEngine;

public class Confirm : Command
{
    [RequiredParameter, ParameterAlias(NamelessParameterAlias), Documentation("Message to show the player.")]
    public StringParameter ConfirmationMessage;
    [ParameterAlias("yes"), Documentation("Label to @goto if the player chooses to preceed.")]
    [ResourceContext(ScriptsConfiguration.DefaultPathPrefix, 0), ConstantContext("Labels/{:Path[0]??$Script}", 1)]
    public NamedStringParameter YesLabel;
    [ParameterAlias("no"), Documentation("Label to @goto if the player chooses not to preceed.")]
    [ResourceContext(ScriptsConfiguration.DefaultPathPrefix, 0), ConstantContext("Labels/{:Path[0]??$Script}", 1)]
    public NamedStringParameter NoLabel;
    [ParameterAlias("reset")]
    public StringListParameter ResetState;

    public override async UniTask ExecuteAsync (AsyncToken asyncToken = default)
    {
        var uiManager = Engine.GetService<IUIManager>();
        var confirmationUI = uiManager.GetUI<IConfirmationUI>();

        var resetState = Assigned(ResetState) ? ResetState : (StringListParameter)new List<string> { Goto.NoResetFlag };

        if (!await confirmationUI.ConfirmAsync(ConfirmationMessage)) 
            await new Goto { Path = NoLabel, ResetState = resetState }.ExecuteAsync(asyncToken);
        else
            await new Goto { Path = YesLabel, ResetState = resetState }.ExecuteAsync(asyncToken);

    }
}