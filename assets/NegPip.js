postParamBuildSteps.push(() => {
    let dynThreshGroup = document.getElementById('input_group_content_negpip');
    if (dynThreshGroup && !currentBackendFeatureSet.includes('negpip')) {
        dynThreshGroup.append(createDiv(`negpip_install_button`, 'keep_group_visible', `<button class="basic-button" onclick="installFeatureById('negpip', 'negpip_install_button')">Install NegPip</button>`));
    }
});
