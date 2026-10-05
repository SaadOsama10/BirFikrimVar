// Clone the last section, clear it, and renumber the fields (PostPlot[0].Text, PostPlot[1].Text, ...).
function addPostPlot() {
    var $last = $('.post-plot .post-plot-item').last();
    var $clone = $last.clone(true);
    $clone.find('.post-plot-text').val('');
    $clone.find('.post-plot-image').val('');
    $clone.find('.post-plot-sort').val('');
    $clone.appendTo('.post-plot');
    reindexPlots();
}

function reindexPlots() {
    $('.post-plot .post-plot-item').each(function (idx) {
        $(this).find('.post-plot-text').attr('name', 'PostPlot[' + idx + '].Text');
        $(this).find('.post-plot-sort').attr('name', 'PostPlot[' + idx + '].Sort');
        $(this).find('.post-plot-image').attr('name', 'PostPlot[' + idx + '].Image');
    });
}

$(function () {
    $('#add-plot').on('click', addPostPlot);
    $('#post-form').on('submit', reindexPlots);
});
