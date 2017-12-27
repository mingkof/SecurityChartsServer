const sharp = require('sharp');

module.exports = function(result, physicalPath, maxWidth) {
   sharp(physicalPath)
       .resize(maxWidth)
       .pipe(result.stream);

}