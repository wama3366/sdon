// Copyright (c) Duende Software. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace IdentityAndAccess.API.Ui.Pages.Consent;

public class InputModel
{
    public string Button { get; set; }
    public IEnumerable<string> ScopesConsented { get; set; } = [];
    public bool RememberConsent { get; set; } = true;
    public string ReturnUrl { get; set; }
    public string Description { get; set; }
}
