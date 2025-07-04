postParamBuildSteps.push(() => {
    let NegPipGroup = document.getElementById('input_group_content_sampling');
    if (NegPipGroup && !currentBackendFeatureSet.includes('negpip')) {
        NegPipGroup.append(createDiv(`negpip_install_button`, 'keep_group_visible', `<button class="basic-button" onclick="installFeatureById('negpip', 'negpip_install_button')">Install NegPip</button>`));
    }
});
