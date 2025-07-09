import {TranslocoGlobalConfig} from '@jsverse/transloco-utils';
import { LanguagesConstant } from './src/app/shared/constants/language/languages.constant';
    
const config: TranslocoGlobalConfig = {
  rootTranslationsPath: 'src/assets/i18n/',
  langs: LanguagesConstant.supportedLanguages,
  keysManager: {},
};
    
export default config;