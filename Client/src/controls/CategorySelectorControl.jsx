import { useController } from "react-hook-form";
import CategorySelector from "../components/CategorySelector";

const CategorySelectorControl = ({
  control,
  name,
  isInvalid,
  amountIsPositive,
}) => {
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
    <CategorySelector
      onChange={field.onChange} // send value to hook form
      onBlur={field.onBlur} // notify when input is touched/blur
      value={field.value} // input value
      name={field.name} // send down the input name
      inputRef={field.ref} // send input ref, so we can focus on input when error appear
      isInvalid={isInvalid}
      amountIsPositive={amountIsPositive}
    />
  );
};

export default CategorySelectorControl;
