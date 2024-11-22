import { useController } from "react-hook-form";
import CurrencyInput from "../components/CurrencyInput";

const CurrencyInputControl = ({ control, name, isInvalid }) => {
  const {
    field,
    fieldState: { invalid, isTouched, isDirty },
    formState: { touchedFields, dirtyFields },
  } = useController({
    name,
    control,
    rules: { required: true },
  });

  return (
    <CurrencyInput
      onChange={field.onChange} // send value to hook form
      onBlur={field.onBlur} // notify when input is touched/blur
      value={field.value} // input value
      name={field.name} // send down the input name
      inputRef={field.ref} // send input ref, so we can focus on input when error appear
      isInvalid={isInvalid}
    />
  );
};

export default CurrencyInputControl;
